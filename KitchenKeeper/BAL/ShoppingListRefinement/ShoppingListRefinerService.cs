using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using System.Data;
using System.Linq;

namespace KitchenKeeper.BAL.ShoppingListRefinement_BAL
{
    public class ShoppingListRefinerService : IShoppingListRefinerService
    {
        private readonly IStock_SQL _stock_SQL;

        public ShoppingListRefinerService(IStock_SQL stock_SQL)
        {
            _stock_SQL = stock_SQL;
        }

        public async Task<ShoppingList> GenerateShoppingListFromRecipe(Recipe recipe)
        {
            List<FoodBase> foodsInStock = await GetFoodByIngredients(recipe.Ingredients);
            //find out which ingredients are missing from the recipe and add them to the ingredients to buy
            ShoppingList shoppingList = new ShoppingList()
            {
                IngredientsInStock = foodsInStock,
                IngredientsToBuy = recipe.Ingredients,
                IsReadyToCook = false
            };
            FilterIngredientsNotInStock(shoppingList);
            return shoppingList;
        }

        private async Task<List<FoodBase>> GetFoodByIngredients(List<Ingredient> ingredients)
        {
            DataTable ingredientNameDT = GenerateDataTableFromIngredientNames(ingredients);
            IEnumerable<Food_DTO> unrefinedFoods = await _stock_SQL.GetFoodByIngredientNameDT(ingredientNameDT);
            List<FoodBase> unrefinedFoodList = Model_Mapper.ConvertDTOListToFoodList(unrefinedFoods).ToList();
            return RefineFoodInStock(unrefinedFoodList);
        }

        private DataTable GenerateDataTableFromIngredientNames(List<Ingredient> ingredients)
        {
            List<string> ingredientNames = ExtractIngredientNamesToList(ingredients);
            return GenerateIngredientNameDataTable(ingredientNames);
        }

        private List<string> ExtractIngredientNamesToList(List<Ingredient> ingredients)
        {
            List<string> ingredientNames = new List<string>();
            foreach (var ingredient in ingredients)
            {
                ingredientNames.Add(ingredient.Name);
            }
            return ingredientNames;
        }

        private DataTable GenerateIngredientNameDataTable(List<string> ingredientNames)
        {
            DataTable ingredientNameDT = new DataTable();
            ingredientNameDT.Columns.Add("Name", typeof(string));
            foreach (var name in ingredientNames)
            {
                ingredientNameDT.Rows.Add(name);
            }
            return ingredientNameDT;
        }

        private static void FilterIngredientsNotInStock(ShoppingList shoppingList)
        {
            if (shoppingList?.IngredientsInStock == null || shoppingList.IngredientsInStock.Count == 0) return;
            if (shoppingList.IngredientsToBuy == null || shoppingList.IngredientsToBuy.Count == 0) return;

            foreach (var stockFood in shoppingList.IngredientsInStock)
            {
                var matching = shoppingList.IngredientsToBuy
                    .FirstOrDefault(i => string.Equals(i.Name, stockFood.Name, StringComparison.OrdinalIgnoreCase));
                if (matching == null) continue;

                if (stockFood.Quantity >= matching.Quantity)
                {
                    // stock covers required amount: set stock quantity to the needed amount and remove ingredient to buy
                    stockFood.Quantity = matching.Quantity;
                    shoppingList.IngredientsToBuy.Remove(matching);
                }
                else
                {
                    // stock partially covers requirement: subtract stock from amount to buy
                    matching.Quantity -= stockFood.Quantity;
                }
            }

            shoppingList.IsReadyToCook = (shoppingList.IngredientsToBuy?.Count ?? 0) == 0;
        }

        private List<FoodBase> RefineFoodInStock(List<FoodBase> unrefinedFoodsInStock)
        {
            //find duplicates and add them together, keeping the old expiration date if they are the same food, and the new expiration date if they are different foods.
            //extract distinct and add to refined foods 
            List<FoodBase> refinedFoodsInStock = ExtractDistinctFoodToList(unrefinedFoodsInStock);
            //remove foods from original list if they are in the refined list,
            foreach (var food in refinedFoodsInStock)
            {
                unrefinedFoodsInStock.RemoveAll(f => f.Name == food.Name);
            }

            //Add the quantities of the duplicate food if there are still values in the unrefined list and add them to the refined
            if (unrefinedFoodsInStock.Count != 0)
            {
                CombineDuplicateFood(unrefinedFoodsInStock);
                refinedFoodsInStock.AddRange(unrefinedFoodsInStock);
            }

            return refinedFoodsInStock;

        }

        private List<FoodBase> ExtractDistinctFoodToList(List<FoodBase> unrefinedFoodsInStock)
        {
            // Return only those foods whose names appear exactly once (case-insensitive)
            var distinctOnly = unrefinedFoodsInStock
                .GroupBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();

            return distinctOnly;
        }


        private static void CombineDuplicateFood(List<FoodBase> foods)
        {
            var groups = GroupFoodsByName(foods);

            foreach (var group in groups)
            {
                // Only combine if there are duplicates
                if (group.Count() <= 1)
                    continue;

                // Find the oldest item
                var oldest = group
                    .OrderBy(f => f.DateAdded)
                    .First();

                // Sum quantities of all items in the group
                double totalQuantity = group.Sum(f => f.Quantity);

                // Assign total quantity to the oldest item
                oldest.Quantity = totalQuantity;

                // Remove all other items from the list
                foreach (var item in group.Where(f => f != oldest))
                {
                    foods.Remove(item);
                }
            }
        }

        private static List<IGrouping<string, FoodBase>> GroupFoodsByName(List<FoodBase> foods)
        {
            return foods
                .GroupBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

    }
}

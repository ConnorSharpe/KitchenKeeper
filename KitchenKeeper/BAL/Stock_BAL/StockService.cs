using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using System;
using System.Data;
using System.Linq;

namespace KitchenKeeper.BAL.Stock_BAL
{
    public class StockService : IStockService
    {
        private readonly IStock_SQL _stock_SQL;

        public StockService(IStock_SQL stock_SQL)
        {
            _stock_SQL = stock_SQL;
        }

        public async Task<int> AddFood(FoodBase food)
        {
            Food_DTO food_DTO = Model_Mapper.ConvertFoodToDTO(food);
            return await _stock_SQL.AddFood(food_DTO);
        }

        public async Task<FoodBase> GetFoodById(int id)
        {
            Food_DTO? food_DTO = await _stock_SQL.GetFoodById(id);
            CheckDTOForNull(food_DTO);
            return Model_Mapper.ConvertDTOToFood(food_DTO);
        }

        public async Task<IEnumerable<FoodBase>> SearchInventoryByName(string name)
        {
            IEnumerable<Food_DTO> foodDTOs = await _stock_SQL.SearchInventoryByName(name);
            return Model_Mapper.ConvertDTOListToFoodList(foodDTOs);
        }

        public async Task<int> UpdateFood(FoodBase food)
        {
            CheckIdForValue(food.ID);

            Food_DTO food_DTO = Model_Mapper.ConvertFoodToDTO(food);
            return await _stock_SQL.UpdateFood(food_DTO);
        }

        public async Task<int> DeleteFood(int id)
        {
            CheckIdForValue(id);

            return await _stock_SQL.DeleteFood(id);
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

        private static void FilterIngredientsNotInStock(ShoppingList shoppingList)
        {
            if (shoppingList == null) return;

            var toBuy = shoppingList.IngredientsToBuy;
            var inStock = shoppingList.IngredientsInStock;

            if (toBuy == null || toBuy.Count == 0) return;
            if (inStock == null || inStock.Count == 0) return;

            // Build a lookup of names in stock for fast comparison (case-insensitive)
            var inStockNames = new HashSet<string>(inStock.Select(f => f.Name), StringComparer.OrdinalIgnoreCase);

            // Remove any ingredient to buy that has a matching name in stock
            toBuy.RemoveAll(i => inStockNames.Contains(i.Name));

            shoppingList.IsReadyToCook = (shoppingList.IngredientsToBuy?.Count ?? 0) == 0;

        }

        private async Task<List<FoodBase>> GetFoodByIngredients(List<Ingredient> ingredients)
        {
            List<string> ingredientNames = ExtractIngredientNamesToList(ingredients);
            DataTable ingredientNameDT = GenerateIngredientNameDataTable(ingredientNames);
            IEnumerable<Food_DTO> unrefinedFoods = await _stock_SQL.GetFoodByIngredientNameDT(ingredientNameDT);
            List<FoodBase> unrefinedFoodList = Model_Mapper.ConvertDTOListToFoodList(unrefinedFoods).ToList();
            return RefineFoodInStock(unrefinedFoodList);
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

        private List<string> ExtractIngredientNamesToList(List<Ingredient> ingredients)
        {
            List<string> ingredientNames = new List<string>();
            foreach (var ingredient in ingredients)
            {
                ingredientNames.Add(ingredient.Name);
            }
            return ingredientNames;
        }


        private List<FoodBase> RefineFoodInStock(List<FoodBase> unrefinedFoodsInStock)
        {
            //find duplicates and add them together, keeping the old expiration date if they are the same food, and the new expiration date if they are different foods.
            //extract distinct and add to refined foods 
            List<FoodBase> refinedFoodsInStock = ExtractDistinctFoodToList(unrefinedFoodsInStock);
            //remove foods from original list if they are in the refined list,
            foreach(var food in refinedFoodsInStock)
            {
                unrefinedFoodsInStock.RemoveAll(f => f.Name == food.Name);
            }

            //Add the quantities of the duplicate food if there are still values in the unrefined list and add them to the refined
            if(unrefinedFoodsInStock.Count != 0)
            {
                CombineDuplicateFood(unrefinedFoodsInStock);

                //TODO: Check distinct
                refinedFoodsInStock.AddRange(unrefinedFoodsInStock);
            }

            return refinedFoodsInStock;

        }

        public static void CombineDuplicateFood(List<FoodBase> foods)
        {
            var groups = foods
                .GroupBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

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


        private List<FoodBase> ExtractDistinctFoodToList(List<FoodBase> unrefinedFoodsInStock)
        {
            List<FoodBase> distinctFood = unrefinedFoodsInStock.DistinctBy(f => new { f.Name }).ToList();
            return distinctFood;
        }



        private void CheckIdForValue(int? id)
        {
            if (id is null or 0)
                throw new Exception("Invalid Food Id");
        }

        private void CheckDTOForNull(Food_DTO? dto)
        {
            if (dto is null)
                throw new Exception("Food not Found");
        }
    }
}

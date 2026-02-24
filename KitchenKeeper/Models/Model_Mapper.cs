using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.Models
{
    public class Model_Mapper
    {
        public static IEnumerable<Food_DTO> ConvertFoodListToDTOList(IEnumerable<FoodBase> foodList)
        {
            List<Food_DTO> dtoList = new List<Food_DTO>();
            foreach (var food in foodList)
            {
                dtoList.Add(ConvertFoodToDTO(food));
            }
            return dtoList;
        }

        public static Food_DTO ConvertFoodToDTO(FoodBase food)
        {

            Food_DTO food_DTO = new Food_DTO
            {
                ID = food.ID,
                Name = food.Name,
                DateAdded = food.DateAdded,
                ExpirationDate = CalculateExpirationIfNull(food.ExpirationDate, food.DateAdded, food.Storage),
                Storage = food.Storage.ToString(),
                Quantity = food.Quantity,
                UnitOfMeasurement = food.UnitOfMeasurement.ToString(),
                Class = food.Class.ToString(),
                Subclass = food.Subclass.ToString(),
                IsCooked = food.IsCooked,
                IsVegetarian = food.IsVegetarian
            };

            return food_DTO;
        }

        private static DateOnly CalculateExpirationIfNull(DateOnly? expirationDate, DateOnly dateAdded, StorageType storageType)
        {
            if (expirationDate.HasValue)
                return (DateOnly)expirationDate;
            else
                return ExpirationCalculator.SetDate(dateAdded, storageType);
        }

        public static IEnumerable<FoodBase> ConvertDTOListToFoodList(IEnumerable<Food_DTO> dtoList)
        {
            List<FoodBase> foodList = new List<FoodBase>();
            foreach (var dto in dtoList)
            {
                foodList.Add(ConvertDTOToFood(dto));
            }
            return foodList;
        }

        public static FoodBase ConvertDTOToFood(Food_DTO food_DTO)
        {
            FoodBase food = new FoodBase
            {
                ID = food_DTO.ID,
                Name = food_DTO.Name,
                DateAdded = food_DTO.DateAdded,
                ExpirationDate = food_DTO.ExpirationDate,
                Storage = Enum.Parse<StorageType>(food_DTO.Storage),
                Quantity = food_DTO.Quantity,
                UnitOfMeasurement = Enum.Parse<QuantityType>(food_DTO.UnitOfMeasurement),
                Class = Enum.Parse<FoodClass>(food_DTO.Class),
                Subclass = Enum.Parse<FoodSubclass>(food_DTO.Subclass),
                IsCooked = food_DTO.IsCooked,
                IsVegetarian = food_DTO.IsVegetarian
            };
            return food;
        }

        public static Recipe_DTO ConvertRecipeToDTO(Recipe recipe)
        {
            return new Recipe_DTO
            {
                RecipeDetails = new Recipe_Details_DTO
                {
                    ID = recipe.ID,
                    Name = recipe.Name
                },
                IngredientsDT = ConvertIngredientsToDataTable(recipe.Ingredients),
                InstructionsDT = ConvertInstructionsToDataTable(recipe.Instructions)
            };
        }

        public static DataTable ConvertIngredientsToDataTable(List<Ingredient> ingredients)
        {
            DataTable ingredientDT = new DataTable();
            ingredientDT.Columns.Add("Name", typeof(string));
            ingredientDT.Columns.Add("Quantity", typeof(double));
            ingredientDT.Columns.Add("UnitOfMeasurement", typeof(string));
            foreach (var ingredient in ingredients)
            {
                ingredientDT.Rows.Add(ingredient.Name, ingredient.Quantity, ingredient.UnitOfMeasurement.ToString());
            }
            return ingredientDT;
        }

        public static DataTable ConvertInstructionsToDataTable(List<Instruction> instructions)
        {
            DataTable instructionDT = new DataTable();
            instructionDT.Columns.Add("Order", typeof(int));
            instructionDT.Columns.Add("Content", typeof(string));
            foreach (var instruction in instructions)
            {
                instructionDT.Rows.Add(instruction.Order, instruction.Content);
            }
            return instructionDT;
        }
    }
}

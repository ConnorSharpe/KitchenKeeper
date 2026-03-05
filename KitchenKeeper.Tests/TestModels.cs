using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.Classes;
using KitchenKeeper.Controllers;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using Moq;

namespace KitchenKeeper.UnitTests
{
    public class TestModels
    {
        private static DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        private static DateOnly twoWeeksFromNow = today.AddDays(14);
        private StockController SetUpStockController(Mock<IStock_SQL> mockSetUp)
        {
            IStockService stockService = new StockService(mockSetUp.Object);
            return new StockController(stockService);
        }


        public Food CreateTestFoodBase()
        {
            return new Food()
            {
                Name = "Test Food",
                DateAdded = today,
                ExpirationDate = twoWeeksFromNow,
                Storage = StorageType.Pantry,
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count,
                Class = FoodClass.Meat,
                IsCooked = false,
                IsVegetarian = false
            };
        }

        public Food_DTO CreateTestFoodDTO()
        {
            return new Food_DTO()
            {
                ID = 1,
                Name = "Test Food",
                DateAdded = today,
                ExpirationDate = twoWeeksFromNow,
                Storage = StorageType.Pantry.ToString(),
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count.ToString(),
                Class = FoodClass.Meal.ToString(),
                Subclass = FoodSubclass.Meal_Meat.ToString(),
                IsCooked = true,
                IsVegetarian = true
            };
        }

        public Recipe CreateTestRecipe()
        {
            return new Recipe()
            {
                ID = 1,
                Name = "Test Recipe",
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        ID = 1,
                        Name = "Onion",
                        RecipeID = 1,
                        Quantity = 1,
                        UnitOfMeasurement = QuantityType.count
                    },
                    new Ingredient()
                    {
                        ID = 2,
                        Name = "Beef",
                        RecipeID = 1,
                        Quantity = 16,
                        UnitOfMeasurement = QuantityType.oz
                    },
                    new Ingredient()
                    {
                        ID = 3,
                        Name = "Stock",
                        RecipeID = 1,
                        Quantity = 12,
                        UnitOfMeasurement = QuantityType.fl_oz
                    },
                    new Ingredient()
                    {
                        ID = 4,
                        Name = "Salt",
                        RecipeID = 1,
                        Quantity = 2,
                        UnitOfMeasurement = QuantityType.gram
                    },
                },
                Instructions = new List<Instruction>(),
            };
        }
    }
}

using Azure.Identity;
using KitchenKeeper.BAL.ShoppingListRefinement_BAL;
using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.Classes;
using KitchenKeeper.Controllers;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace KitchenKeeper.UnitTests
{
    public class ShoppingListTests
    {
        private static DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        private static DateOnly twoWeeksFromNow = today.AddDays(14);

        private FoodBase CreateTestFoodBase()
        {
            return new FoodBase()
            {
                Name = "Test Food",
                DateAdded = today,
                ExpirationDate = twoWeeksFromNow,
                Storage = StorageType.Shelf,
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count,
                Class = FoodClass.Meat,
                IsCooked = false,
                IsVegetarian = false
            };
        }
        private Food_DTO CreateTestFoodDTO(string name, int quantity)
        {
            return new Food_DTO()
            {
                ID = 1,
                Name = name,
                DateAdded = today,
                ExpirationDate = twoWeeksFromNow,
                Storage = StorageType.Shelf.ToString(),
                Quantity = quantity,
                UnitOfMeasurement = QuantityType.count.ToString(),
                Class = FoodClass.Meal.ToString(),
                Subclass = FoodSubclass.Meal.ToString(),
                IsCooked = true,
                IsVegetarian = true
            };
        }

        

        private Recipe CreateTestRecipe()
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
                        Name = "BeefStock",
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

        //private IEnumerable<Food_DTO> CreateTestFoodFromStock(Dictionary<string,int> testValues)
        //{
        //    List<Food_DTO> testFoodFromStockList = new List<Food_DTO>();

            
        //    foreach (var testValue in testValues)
        //    {
        //        Food_DTO testFoodDTO = CreateTestFoodDTO(testValue.Key, testValue.Value);
        //        testFoodFromStockList.Add(testFoodDTO);
        //    }

        //    IEnumerable<Food_DTO> testFoodFromStock = testFoodFromStockList;
        //    return testFoodFromStock;
        //}

        private ShoppingListController SetUpControllerWithMock(IEnumerable<Food_DTO> specificStockItemsToTest)
        {
            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.GetFoodByIngredientNameDT(It.IsAny<DataTable>()))
                    .ReturnsAsync(specificStockItemsToTest);

            return SetUpShoppinglistController(stockSqlMock);
        }
        

        private ShoppingListController SetUpShoppinglistController(Mock<IStock_SQL> mockSetUp)
        {
            IShoppingListRefinerService shoppingListRefinerService = new ShoppingListRefinerService(mockSetUp.Object);
            return new ShoppingListController(shoppingListRefinerService);
        }
        #region ShoppingList

        [Fact]
        public async Task GenerateShoppingListAsync()
        {
            // Arrange
            Recipe testRecipe = CreateTestRecipe();
            IEnumerable<Food_DTO> specificStockItemsToTest = new List<Food_DTO>()
            {
                CreateTestFoodDTO("Beef", 12),
                CreateTestFoodDTO("Onion", 2)
            };

            var shoppingListController = SetUpControllerWithMock(specificStockItemsToTest);
            // Act
            IActionResult result = await shoppingListController.GenerateShoppingListFromRecipe(testRecipe);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RefineShoppingListCombinesDuplicateStockAsync()
        {
            Recipe testRecipe = CreateTestRecipe();

            IEnumerable<Food_DTO> specificStockItemsToTest = new List<Food_DTO>()
            {
                CreateTestFoodDTO("Beef", 12),
                CreateTestFoodDTO("Beef", 3)
            };


            double expectedBeefQuantityInStock = specificStockItemsToTest.Where(i => i.Name == "Beef").Sum(i => i.Quantity);
            double expectedBeefQuantityToBuy = testRecipe.Ingredients.Where(x => x.Name == "Beef").Sum(i => i.Quantity) - expectedBeefQuantityInStock;

            var shoppingListController = SetUpControllerWithMock(specificStockItemsToTest);

            IActionResult result = await shoppingListController.GenerateShoppingListFromRecipe(testRecipe);
            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var shoppingList = Assert.IsType<ShoppingList>(ok.Value);

            //Creates single entity
            Assert.Single(shoppingList.IngredientsInStock);
            Assert.Equal("Beef", shoppingList.IngredientsInStock[0].Name);

            double actualBeefQuantityInStock = shoppingList.IngredientsInStock.Where(f => f.Name == "Beef").Sum(f => f.Quantity);
            double actualBeefQuantityToBuy = shoppingList.IngredientsToBuy.Where(f => f.Name == "Beef").Sum(f => f.Quantity);
            Assert.Equal(expectedBeefQuantityInStock, actualBeefQuantityInStock);
            Assert.Equal(actualBeefQuantityToBuy, expectedBeefQuantityToBuy);

            
        }

        #endregion
    }
}

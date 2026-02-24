using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.Classes;
using KitchenKeeper.Controllers;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Data;
using System.Data.Common;

namespace KitchenKeeper.UnitTests
{
    public class StockTests
    {
        #region Create Models
        private static DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        private static DateOnly twoWeeksFromNow = today.AddDays(14);

        private StockController SetUpStockController(Mock<IStock_SQL> mockSetUp)
        {
            IStockService stockService = new StockService(mockSetUp.Object);
            return new StockController(stockService);
        }


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

        private Food_DTO CreateTestFoodDTO()
        {
            return new Food_DTO()
            {
                ID = 1,
                Name = "Test Food",
                DateAdded = today,
                ExpirationDate = twoWeeksFromNow,
                Storage = StorageType.Shelf.ToString(),
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count.ToString(),
                Class = FoodClass.Meal.ToString(),
                Subclass = FoodSubclass.Meal.ToString(),
                IsCooked = true,
                IsVegetarian = true
            };
        }

        

        #endregion

        #region Model Tests

        [Fact]
        public void ConvertFoodBaseToDTOExpirationDateIsNotNull()
        {
            // Arrange
            FoodBase testFood = CreateTestFoodBase();
            testFood.ExpirationDate = null;

            // Expected expiration when null should be calculated from DateAdded
            DateOnly expectedExpiration = ExpirationCalculator.SetDate(testFood.DateAdded, testFood.Storage);

            // Act
            var testFoodDTO = Model_Mapper.ConvertFoodToDTO(testFood);

            // Assert
            Assert.NotNull(testFoodDTO);
            Assert.Equal(testFood.Name, testFoodDTO.Name);
            Assert.Equal(testFood.DateAdded, testFoodDTO.DateAdded);
            Assert.Equal(expectedExpiration, testFoodDTO.ExpirationDate);
            Assert.Equal(testFood.Storage.ToString(), testFoodDTO.Storage);
            Assert.Equal(testFood.Quantity, testFoodDTO.Quantity);
            Assert.Equal(testFood.UnitOfMeasurement.ToString(), testFoodDTO.UnitOfMeasurement);
        }

        [Fact]
        public void ShoppingListIsReadyToCookWhenIngredientsToBuyIsEmpty()
        {
            // Arrange
            ShoppingList shoppingList = new ShoppingList();
            // Act
            bool isReadyToCook = shoppingList.IsReadyToCook;
            // Assert
            Assert.True(shoppingList.IsReadyToCook);
        }
        #endregion

        #region Add food

        [Fact]
        public async Task AddFoodAsync()
        {

            // Arrange
            FoodBase testFood = CreateTestFoodBase();
            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.AddFood(It.IsAny<Food_DTO>()))
                .ReturnsAsync(1); // return a positive id

            var stockController = SetUpStockController(stockSqlMock);

            // Act
            IActionResult result = await stockController.AddFood(testFood);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, ((OkObjectResult)result).Value);

            // Optional: verify the SQL method was called once
            stockSqlMock.Verify(s => s.AddFood(It.IsAny<Food_DTO>()), Times.Once);
        }
        #endregion

        #region Get Food

        [Fact]
        public async Task SearchInventoryByNameAsync()
        {
            // Arrange
            string searchName = "Test";
            IEnumerable<Food_DTO> expectedFoods = new List<Food_DTO>
            {
                CreateTestFoodDTO(),
            };
            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.SearchInventoryByName(It.IsAny<string>()))
                .ReturnsAsync(expectedFoods);
            var stockController = SetUpStockController(stockSqlMock);
            // Act
            IActionResult result = await stockController.SearchInventoryByName(searchName);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(((OkObjectResult)result).Value);

            // Optional: verify the SQL method was called once with correct parameters
            stockSqlMock.Verify(s => s.SearchInventoryByName(searchName), Times.Once);
        }

        [Fact]
        public async Task GetFoodByIdAsync()
        {
            // Arrange
            int foodId = 1;
            Food_DTO expectedFood = CreateTestFoodDTO();
            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.GetFoodById(It.IsAny<int>()))
                .ReturnsAsync(expectedFood);
            var stockController = SetUpStockController(stockSqlMock);
            // Act
            IActionResult result = await stockController.GetFoodById(foodId);
            // Assert
            Assert.NotNull(((OkObjectResult)result).Value);
            // Optional: verify the SQL method was called once with correct parameters
            stockSqlMock.Verify(s => s.GetFoodById(foodId), Times.Once);
        }

        #endregion

        #region Update Food
        [Fact]
        public async Task UpdateFoodAsync()
        {
            // Arrange
            FoodBase testFood = CreateTestFoodBase();
            testFood.ID = 123;

            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.UpdateFood(It.IsAny<Food_DTO>()))
                .ReturnsAsync(1); // return a positive id

            var stockController = SetUpStockController(stockSqlMock);
            // Act
            IActionResult result = await stockController.UpdateFood(testFood);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            // Optional: verify the SQL method was called once with correct parameters
            stockSqlMock.Verify(s => s.UpdateFood(It.IsAny<Food_DTO>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFoodThrowsErrorIfIdIsNullOrZero()
        {
            // Arrange
            FoodBase testFood = CreateTestFoodBase();
            testFood.ID = 0; // Invalid ID
            var stockSqlMock = new Mock<IStock_SQL>();
            var stockController = SetUpStockController(stockSqlMock);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => stockController.UpdateFood(testFood));
            testFood.ID = 0; // Invalid ID
            await Assert.ThrowsAsync<Exception>(() => stockController.UpdateFood(testFood));
        }

        #endregion

        #region Delete Food
        [Fact]
        public async Task DeleteFoodAsync()
        {
            // Arrange
            int id = 123;
            var stockSqlMock = new Mock<IStock_SQL>();
            stockSqlMock
                .Setup(s => s.DeleteFood(It.IsAny<int>()))
                    .ReturnsAsync(1); // return a positive id
            var stockController = SetUpStockController(stockSqlMock);

            // Act
            IActionResult result = await stockController.DeleteFood(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            // Optional: verify the SQL method was called once
            stockSqlMock.Verify(s => s.DeleteFood(123), Times.Once);
        }
        #endregion
    }
}

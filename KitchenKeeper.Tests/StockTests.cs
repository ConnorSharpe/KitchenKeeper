using KitchenKeeper.BAL.ExpirationCalculator_BAL;
using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.Classes;
using KitchenKeeper.Controllers;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

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


        private Food CreateTestFoodBase()
        {
            return new Food()
            {
                Name = "Test Food",
                DateAdded = today,
                ExpirationDate = null,
                Storage = StorageType.Refrigerator,
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count,
                Class = FoodClass.Meal,
                Subclass = FoodSubclass.Meal_Meat,
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
                ExpirationDate = ExpirationCalculator.Calculate(FoodSubclass.Meal_Meat, StorageType.Refrigerator),
                Storage = StorageType.Pantry.ToString(),
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count.ToString(),
                Class = FoodClass.Meal.ToString(),
                Subclass = FoodSubclass.Meal_Meat.ToString(),
                IsCooked = true,
                IsVegetarian = true
            };
        }

        

        #endregion

        #region Model Tests

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
            Food testFood = CreateTestFoodBase();
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

        [Fact]
        public async Task AddedFoodExpirationDateIsNotNull()
        {
            // Arrange
            Food testFood = CreateTestFoodBase();
            DateOnly? expectedDate = ExpirationCalculator.Calculate(testFood.Subclass, testFood.Storage);
            testFood.ExpirationDate = null; // Ensure ExpirationDate is null to test if it's set in the service
            Food_DTO? captured = null;
            var stockSqlMock = new Mock<IStock_SQL>();

            stockSqlMock
                .Setup(s => s.AddFood(It.IsAny<Food_DTO>()))
                .Callback<Food_DTO>(dto => captured = dto)
                .ReturnsAsync(1);

            var stockController = SetUpStockController(stockSqlMock);


            await stockController.AddFood(testFood);

            Assert.NotNull(captured);
            Assert.Equal(expectedDate, captured.ExpirationDate); // or Assert.True(captured.ExpirationDate.HasValue)
        }

        [Fact]
        public async Task AddFoodThrowsErrorIfExpirationIsNullAndCannotBeCalculated()
        {
            // Arrange
            Food testFood = CreateTestFoodBase();
            testFood.ExpirationDate = null; // Ensure ExpirationDate is null to test if it's set in the service
            testFood.Storage = StorageType.Pantry;
            var stockSqlMock = new Mock<IStock_SQL>();
            var stockController = SetUpStockController(stockSqlMock);
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => stockController.AddFood(testFood));
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
            Food testFood = CreateTestFoodBase();
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
            Food testFood = CreateTestFoodBase();
            testFood.ID = 0; // Invalid ID
            var stockSqlMock = new Mock<IStock_SQL>();
            var stockController = SetUpStockController(stockSqlMock);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => stockController.UpdateFood(testFood));
            testFood.ID = 0; // Invalid ID
            await Assert.ThrowsAsync<Exception>(() => stockController.UpdateFood(testFood));
        }

        [Fact]
        public async Task UpdatedFoodExpirationDateIsNotNull()
        {
            // Arrange
            Food testFood = CreateTestFoodBase();
            testFood.ID = 123;
            testFood.ExpirationDate = null; // Ensure ExpirationDate is null to test if it's set in the service

            DateOnly? expectedDate = ExpirationCalculator.Calculate(testFood.Subclass, testFood.Storage);
            Food_DTO? captured = null;
            var stockSqlMock = new Mock<IStock_SQL>();

            stockSqlMock
                .Setup(s => s.UpdateFood(It.IsAny<Food_DTO>()))
                .Callback<Food_DTO>(dto => captured = dto)
                .ReturnsAsync(1);

            var stockController = SetUpStockController(stockSqlMock);


            await stockController.UpdateFood(testFood);

            Assert.NotNull(captured);
            Assert.Equal(expectedDate, captured.ExpirationDate); // or Assert.True(captured.ExpirationDate.HasValue)
        }

        [Fact]
        public async Task UpdateFoodThrowsErrorIfExpirationIsNullAndCannotBeCalculated()
        {
            // Arrange
            Food testFood = CreateTestFoodBase();
            testFood.ID = 123;
            testFood.ExpirationDate = null; // Ensure ExpirationDate is null to test if it's set in the service
            testFood.Storage = StorageType.Pantry;
            var stockSqlMock = new Mock<IStock_SQL>();
            var stockController = SetUpStockController(stockSqlMock);
            // Act & Assert
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

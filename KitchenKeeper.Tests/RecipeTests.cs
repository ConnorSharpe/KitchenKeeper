using KitchenKeeper.BAL.RecipeManager;
using KitchenKeeper.Classes;
using KitchenKeeper.Controllers;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Recipe_SQL;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace KitchenKeeper.UnitTests
{
    public class RecipeTests
    {
        private RecipeController SetUpRecipeController(Mock<IRecipe_SQL> mockSetUp)
        {
            IRecipeService recipeService = new RecipeService(mockSetUp.Object);
            return new RecipeController(recipeService);
        }

        private Recipe CreateTestRecipe(int ingredientNumber, int instructionNumber)
        {
            return new Recipe()
            {
                ID = 1,
                Name = "Test Recipe",
                Ingredients = CreateTestIngredientList(ingredientNumber),
                Instructions = CreateTestInstructionList(instructionNumber)
            };
        }

        private List<Ingredient> CreateTestIngredientList(int ingredientNumber)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();

            for (int i = 0; i < ingredientNumber; i++)
            {
                Ingredient ingredient = CreateTestIngredient(i);
                ingredientList.Add(ingredient);
            }

            return ingredientList;
        }

        private Ingredient CreateTestIngredient(int ingredientNumber)
        {
            return new Ingredient()
            {
                ID = ingredientNumber,
                RecipeID = 1,
                Name = "Test Ingredient " + ingredientNumber,
                Quantity = 1,
                UnitOfMeasurement = QuantityType.count
            };
        }

        private List<Instruction> CreateTestInstructionList(int instructionNumber)
        {
            List<Instruction> instructionList = new List<Instruction>();

            for (int i=0; i < instructionNumber; i++)
            {
                Instruction testInstruction = CreateTestInstruction(i);
                instructionList.Add(testInstruction);
            }

            return instructionList;
        }

        private Instruction CreateTestInstruction(int instructionNumber)
        {
            return new Instruction()
            {
                ID = instructionNumber,
                RecipeID = 1,
                Content = "Test Instruction " + instructionNumber,
                Order = instructionNumber
            };
        }

        [Fact]
        public async Task AddRecipeAsync()
        {
            // Arrange
            Mock<IRecipe_SQL> recipeSQLMock = new Mock<IRecipe_SQL>();
            recipeSQLMock
                .Setup(m => m.AddRecipe(It.IsAny<Recipe_DTO>()))
                .ReturnsAsync(1);

            RecipeController recipeController = SetUpRecipeController(recipeSQLMock);
            Recipe testRecipe = CreateTestRecipe(3, 3);
            // Act
            IActionResult result = await recipeController.AddRecipe(testRecipe);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testRecipe.ID, ((OkObjectResult)result).Value);
            recipeSQLMock.Verify(m => m.AddRecipe(It.IsAny<Recipe_DTO>()), Times.Once);
        }
    }
}

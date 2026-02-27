using KitchenKeeper.BAL.RecipeManager;
using KitchenKeeper.Classes;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost(Name = "AddRecipe")]
        public async Task<IActionResult> AddRecipe(Recipe recipe)
        {
            try
            {
                var result = await _recipeService.AddRecipe(recipe);
                return result > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPut(Name = "UpdateRecipe")]
        public async Task<IActionResult> UpdateRecipe(Recipe recipe) 
        {
            try
            {
                int result = await _recipeService.UpdateRecipe(recipe);
                return result > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPut(Name = "UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient(Ingredient ingredient)
        {
            try
            {
                int result = await _recipeService.UpdateIngredient(ingredient);
                return result > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            { 
                Console.Write(ex.Message);
                throw;
            }
        }

        [HttpPut(Name = "UpdateInstruction")]
        public async Task<IActionResult> UpdateInstruction(Instruction instruction)
        {
            try
            {
                int result = await _recipeService.UpdateInstruction(instruction);
                return result > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [HttpGet(Name = "SearchRecipesByName")]
        public async Task<IActionResult> SearchRecipesByName(string name)
        {
            try
            {
                var result = await _recipeService.SearchRecipesByName(name);
                return result.Count() > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet(Name = "SearchRecipesByIngredients")]
        public async Task<IActionResult> SearchRecipesByIngredients(List<string> ingredients)
        {
            try
            {
                var result = await _recipeService.SearchRecipesByIngredients(ingredients);
                return result.Count() > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

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
    }
}

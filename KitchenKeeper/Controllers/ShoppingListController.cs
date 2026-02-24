using KitchenKeeper.BAL.ShoppingListRefinement_BAL;
using KitchenKeeper.Classes;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListRefinerService _shoppingListRefinerService;

        public ShoppingListController(IShoppingListRefinerService shoppingListRefinerService)
        {
            _shoppingListRefinerService = shoppingListRefinerService;
        }

        [HttpGet(Name = "GenerateShoppingListFromRecipe")]
        public async Task<IActionResult> GenerateShoppingListFromRecipe(Recipe recipe)
        {
            try
            {
                var result = await _shoppingListRefinerService.GenerateShoppingListFromRecipe(recipe);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

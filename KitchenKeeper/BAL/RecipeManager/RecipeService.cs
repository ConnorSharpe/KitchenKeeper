using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Recipe_SQL;
using KitchenKeeper.Models;

namespace KitchenKeeper.BAL.RecipeManager
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipe_SQL _recipe_SQL;

        public RecipeService(IRecipe_SQL recipe_SQL)
        {
            _recipe_SQL = recipe_SQL;
        }

        public Task<int> AddRecipe(Recipe recipe)
        {
            Recipe_DTO recipe_DTO = Model_Mapper.ConvertRecipeToDTO(recipe);
            return _recipe_SQL.AddRecipe(recipe_DTO);
        }
    }
}

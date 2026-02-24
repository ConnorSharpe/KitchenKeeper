using KitchenKeeper.Classes;

namespace KitchenKeeper.BAL.RecipeManager
{
    public interface IRecipeService
    {
        Task<int> AddRecipe(Recipe recipe);
    }
}

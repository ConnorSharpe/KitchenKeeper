using KitchenKeeper.Classes;

namespace KitchenKeeper.BAL.RecipeManager
{
    public interface IRecipeService
    {
        Task<int> AddRecipe(Recipe recipe);
        Task<int> UpdateRecipe(Recipe recipe);
        Task<int> UpdateIngredient(Ingredient ingredient);
        Task<int> UpdateInstruction(Instruction instruction);
        Task<IEnumerable<Recipe>> SearchRecipesByName(string name);
        Task<IEnumerable<Recipe>> SearchRecipesByIngredients(List<string> ingredients);
    }
}

using KitchenKeeper.DAL.DTO;

namespace KitchenKeeper.DAL.Recipe_SQL
{
    public interface IRecipe_SQL
    {
        Task<int> AddRecipe(Recipe_DTO recipe_DTO);
        Task<int> UpdateRecipe(Recipe_DTO recipe_DTO);
        Task<int> UpdateIngredient(Ingredient_DTO ingredient_DTO);
        Task<int> UpdateInstruction(Instruction_DTO ingredient_DTO);
        Task<IEnumerable<Recipe_DTO>> SearchRecipesByName(string name);
        Task<IEnumerable<Recipe_DTO>> SearchRecipesByIngredients(List<string> ingredients);
    }
}

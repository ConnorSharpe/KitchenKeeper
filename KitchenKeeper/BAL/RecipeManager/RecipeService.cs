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

        public Task<int> UpdateRecipe(Recipe recipe)
        {
            
            Recipe_DTO recipe_DTO = Model_Mapper.ConvertRecipeToDTO(recipe);
            return _recipe_SQL.UpdateRecipe(recipe_DTO);
        }

        public async Task<int> UpdateIngredient(Ingredient ingredient)
        {
            CheckIdForValue(ingredient.ID);
            
            Ingredient_DTO ingredient_DTO = Model_Mapper.ConvertIngredientToDTO(ingredient);
            return await _recipe_SQL.UpdateIngredient(ingredient_DTO);
        }

        public async Task<int> UpdateInstruction(Instruction instruction)
        {
            CheckIdForValue(instruction.ID);

            Instruction_DTO instruction_DTO = Model_Mapper.ConvertInstructionToDTO(instruction);
            return await _recipe_SQL.UpdateInstruction(instruction_DTO);
        }

        public async Task<IEnumerable<Recipe>> SearchRecipesByName(string name)
        {
            IEnumerable<Recipe_DTO> recipeDTOs = await _recipe_SQL.SearchRecipesByName(name);
            IEnumerable<Recipe> recipes = Model_Mapper.ConvertDTOListToRecipeList(recipeDTOs);
            return recipes;
        }

        public async Task<IEnumerable<Recipe>> SearchRecipesByIngredients(List<string> ingredientNames)
        {
            IEnumerable<Recipe_DTO> recipeDTOs = await _recipe_SQL.SearchRecipesByIngredients(ingredientNames);
            IEnumerable<Recipe> recipes = Model_Mapper.ConvertDTOListToRecipeList(recipeDTOs);
            return recipes;
        }

        private void ValidateRecipe(Recipe recipe)
        {
            CheckIdForValue(recipe.ID);
            // usage
            CheckListForNullID(recipe.Ingredients, i => i.ID, "Ingredients");
            CheckListForNullID(recipe.Instructions, i => i.ID, "Instructions");
        }

        private void CheckListForNullID<T>(IEnumerable<T> list, Func<T, int?> idSelector, string name)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Any(x => idSelector(x) is null || idSelector(x) == 0))
                throw new ArgumentException($"{name} contains items with missing or invalid IDs");
        }

        private void CheckIdForValue(int? id)
        {
            if (id is null or 0)
                throw new Exception("Invalid Food Id");
        }
    }
}

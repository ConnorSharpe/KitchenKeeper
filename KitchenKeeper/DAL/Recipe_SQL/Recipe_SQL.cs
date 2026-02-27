
using Dapper;
using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.DAL.Recipe_SQL
{
    public class Recipe_SQL : IRecipe_SQL
    {
        #region Stored Procedures
        private const string INGREDIENT_UPDATE = "dbo.INGREDIENT_UPDATE";
        private const string INSTRUCTION_UPDATE = "dbo.INSTRUCTION_UPDATE";
        private const string RECIPE_ADD = "dbo.RECIPE_ADD";
        private const string RECIPE_DELETE = "dbo.RECIPE_DELETE";
        //TODO Add DB Work for following stored procedures:
        private const string RECIPE_UPDATE = "dbo.RECIPE_UPDATE";
        private const string RECIPE_SEARCH_BY_NAME = "dbo.RECIPE_SEARCH_BY_NAME";
        private const string RECIPE_SEARCH_BY_INGREDIENTS = "dbo.RECIPE_SEARCH_BY_INGREDIENTS";

        #endregion

        private readonly IDbConnection _db;

        public Recipe_SQL(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> AddRecipe(Recipe_DTO recipe_DTO)
        {
            return await _db.ExecuteScalarAsync<int>(
                RECIPE_ADD,
                recipe_DTO,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateRecipe(Recipe_DTO recipe_DTO)
        {
            return await _db.ExecuteAsync(
                RECIPE_UPDATE,
                recipe_DTO,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateIngredient(Ingredient_DTO ingredient_DTO)
        {
            return await _db.ExecuteAsync(
                INGREDIENT_UPDATE,
                ingredient_DTO,
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> UpdateInstruction(Instruction_DTO instruction_DTO)
        {
            return await _db.ExecuteAsync(
                INSTRUCTION_UPDATE,
                instruction_DTO,
                commandType: CommandType.StoredProcedure
                );
        }

        public Task<IEnumerable<Recipe_DTO>> SearchRecipesByName(string name)
        {
            return _db.QueryAsync<Recipe_DTO>(
                RECIPE_SEARCH_BY_NAME,
                new { NameFragment = name },
                commandType: CommandType.StoredProcedure
            );
        }

        public Task<IEnumerable<Recipe_DTO>> SearchRecipesByIngredients(List<string> ingredientNames)
        {
            string ingredientNamesString = string.Join(",", ingredientNames);
            return _db.QueryAsync<Recipe_DTO>(
                RECIPE_SEARCH_BY_INGREDIENTS,
                new { IngredientNames = ingredientNamesString },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

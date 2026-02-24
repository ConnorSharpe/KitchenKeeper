
using Dapper;
using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.DAL.Recipe_SQL
{
    public class Recipe_SQL: IRecipe_SQL
    {
        #region Stored Procedures
        private const string INGREDIENT_UPDATE = "dbo.INGREDIENT_UPDATE";
        private const string INSTRUCTION_UPDATE = "dbo.INSTRUCTION_UPDATE";
        private const string RECIPE_ADD = "dbo.RECIPE_ADD";
        private const string RECIPE_DELETE = "dbo.RECIPE_DELETE";
        #endregion

        private readonly IDbConnection _db;

        public Recipe_SQL(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> AddRecipe(Recipe_DTO recipe)
        {
            return await _db.ExecuteScalarAsync<int>(
                RECIPE_ADD,
                recipe,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

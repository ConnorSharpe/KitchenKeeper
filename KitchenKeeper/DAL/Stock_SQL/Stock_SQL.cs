using Dapper;
using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.DAL.Stock_SQL
{
    public class Stock_SQL: IStock_SQL
    {
        #region Stored Procedures
        private const string FOOD_ADD = "dbo.FOOD_ADD";
        private const string FOOD_DELETE = "dbo.FOOD_DELETE";
        private const string FOOD_GET_BY_ID = "dbo.FOOD_GET_BY_ID";
        private const string FOOD_GET_BY_INGREDIENTS = "dbo.FOOD_GET_BY_INGREDIENTS";
        private const string FOOD_GET_EXPIRING = "dbo.FOOD_GET_EXPIRING";
        private const string FOOD_SEARCH_BY_NAME = "dbo.FOOD_SEARCH_BY_NAME";
        private const string FOOD_UPDATE = "dbo.FOOD_UPDATE";

        #endregion

        private readonly IDbConnection _db;

        public Stock_SQL(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Food_DTO>> SearchInventoryByName(string name)
        {
            return await _db.QueryAsync<Food_DTO>(
                FOOD_SEARCH_BY_NAME,
                new { NameFragment = name },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Food_DTO?> GetFoodById(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Food_DTO>(
                FOOD_GET_BY_ID,
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddFood(Food_DTO food)
        {
            return await _db.ExecuteScalarAsync<int>(
                FOOD_ADD, 
                food, 
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> UpdateFood(Food_DTO food)
        {
            return await _db.ExecuteAsync(
                FOOD_UPDATE,
                food,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteFood(int id)
        {
            return await _db.ExecuteAsync(
                FOOD_DELETE,
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Food_DTO>> GetFoodByIngredientNameDT(DataTable ingredientNameDT)
        {
            return await _db.QueryAsync<Food_DTO>(
                FOOD_GET_BY_INGREDIENTS,
                new { Ingredients = ingredientNameDT.AsTableValuedParameter("dbo.StringList") },
                commandType: CommandType.StoredProcedure
            );
        }


    }
}

// Using Dapper to call a stored procedure that accepts a table-valued parameter
// (create a user-defined table type in SQL Server, e.g. dbo.StringList with a single nvarchar column named Value)
//
// var ingredientNames = new DataTable();
// ingredientNames.Columns.Add("Value", typeof(string));
// ingredientNames.Rows.Add("chicken");
// ingredientNames.Rows.Add("garlic");
// ingredientNames.Rows.Add("rice");
//
// // If the SP returns rows you can use QueryAsync<T>:
// var results = await _db.QueryAsync<Food_DTO>(
//     FOOD_GET_BY_INGREDIENTS,
//     new { Ingredients = ingredientNames.AsTableValuedParameter("dbo.StringList") },
//     commandType: CommandType.StoredProcedure
// );
//
// // Or, if the SP only performs an action, use ExecuteAsync:
// await _db.ExecuteAsync(
//     FOOD_GET_BY_INGREDIENTS,
//     new { Ingredients = ingredientNames.AsTableValuedParameter("dbo.StringList") },
//     commandType: CommandType.StoredProcedure
// );
//
// Notes:
// - The AsTableValuedParameter extension is provided by Dapper and allows passing a DataTable as a TVP.
// - Ensure the user-defined table type name ("dbo.StringList") matches the SQL Server definition.

using KitchenKeeper.DAL.DTO;

namespace KitchenKeeper.DAL.Recipe_SQL
{
    public interface IRecipe_SQL
    {
        Task<int> AddRecipe(Recipe_DTO recipe_DTO);
    }
}

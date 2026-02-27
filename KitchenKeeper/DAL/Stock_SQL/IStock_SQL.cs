using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.DAL.Stock_SQL
{
    public interface IStock_SQL
    {
        Task<int> AddFood(Food_DTO food);
        Task<Food_DTO?> GetFoodById(int id);
        Task<IEnumerable<Food_DTO>> SearchInventoryByName(string name);
        Task<int> UpdateFood(Food_DTO food);
        Task<int> DeleteFood(int id);
        Task<IEnumerable<Food_DTO>> GetFoodByIngredientNameDT(DataTable ingredientNameDT);
    }
}

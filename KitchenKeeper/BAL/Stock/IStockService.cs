using KitchenKeeper.Classes;

namespace KitchenKeeper.BAL.Stock_BAL
{
    public interface IStockService
    {
        Task<int> AddFood(Food food);
        Task<IEnumerable<Food>> SearchInventoryByName(string name);
        Task<Food> GetFoodById(int id);
        Task<int> DeleteFood(int id);
        Task<int> UpdateFood(Food food);
    }
}

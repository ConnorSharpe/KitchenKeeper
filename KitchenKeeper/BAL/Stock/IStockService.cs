using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.Models;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKeeper.BAL.Stock_BAL
{
    public interface IStockService
    {
        Task<int> AddFood(FoodBase food);
        Task<IEnumerable<FoodBase>> SearchInventoryByName(string name);
        Task<FoodBase> GetFoodById(int id);
        Task<int> DeleteFood(int id);
        Task<int> UpdateFood(FoodBase food);
    }
}

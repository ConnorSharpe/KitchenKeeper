using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using KitchenKeeper.DAL.Stock_SQL;
using KitchenKeeper.Models;
using KitchenKeeper.BAL.ExpirationCalculator_BAL;

namespace KitchenKeeper.BAL.Stock_BAL
{
    public class StockService : IStockService
    {
        private readonly IStock_SQL _stock_SQL;

        public StockService(IStock_SQL stock_SQL)
        {
            _stock_SQL = stock_SQL;
        }

        public async Task<int> AddFood(Food food)
        {
            if (food.ExpirationDate == null)
                food.ExpirationDate = ExpirationCalculator.Calculate(food.Subclass, food.Storage);
            Food_DTO food_DTO = Model_Mapper.ConvertFoodToDTO(food);
            return await _stock_SQL.AddFood(food_DTO);
        }

        public async Task<Food> GetFoodById(int id)
        {
            Food_DTO? food_DTO = await _stock_SQL.GetFoodById(id);
            CheckDTOForNull(food_DTO);
            return Model_Mapper.ConvertDTOToFood(food_DTO);
        }

        public async Task<IEnumerable<Food>> SearchInventoryByName(string name)
        {
            IEnumerable<Food_DTO> foodDTOs = await _stock_SQL.SearchInventoryByName(name);
            return Model_Mapper.ConvertDTOListToFoodList(foodDTOs);
        }

        public async Task<int> UpdateFood(Food food)
        {
            CheckIdForValue(food.ID);

            if (food.ExpirationDate == null)
                food.ExpirationDate = ExpirationCalculator.Calculate(food.Subclass, food.Storage);

            Food_DTO food_DTO = Model_Mapper.ConvertFoodToDTO(food);
            return await _stock_SQL.UpdateFood(food_DTO);
        }

        public async Task<int> DeleteFood(int id)
        {
            CheckIdForValue(id);

            return await _stock_SQL.DeleteFood(id);
        }

        private void CheckIdForValue(int? id)
        {
            if (id is null or 0)
                throw new Exception("Invalid Food Id");
        }

        private void CheckDTOForNull(Food_DTO? dto)
        {
            if (dto is null)
                throw new Exception("Food not Found");
        }
    }
}

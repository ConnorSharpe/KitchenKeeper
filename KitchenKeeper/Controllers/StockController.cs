using KitchenKeeper.BAL.ShoppingListRefinement_BAL;
using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.Classes;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKeeper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet(Name = "SearchInventoryByName")]
        public async Task<IActionResult> SearchInventoryByName(string name)
        {
            try
            {
                var result = await _stockService.SearchInventoryByName(name);
                return result.Count() > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet(Name = "GetFoodById")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            try
            {
                var result = await _stockService.GetFoodById(id);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost(Name = "AddFood")]
        public async Task<IActionResult> AddFood(FoodBase food)
        {
            try
            {
                var result = await _stockService.AddFood(food);
                return result > 0 ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPut(Name = "UpdateFood")]
        public async Task<IActionResult> UpdateFood(FoodBase food)
        {
            try
            {
                int rows = await _stockService.UpdateFood(food);
                return rows > 0 ? Ok(rows) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpDelete(Name = "DeleteFood")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            try
            {
                int rows = await _stockService.DeleteFood(id);
                return rows > 0 ? Ok(rows) : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

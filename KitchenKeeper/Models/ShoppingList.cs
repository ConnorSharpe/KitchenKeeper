using KitchenKeeper.Classes;

namespace KitchenKeeper.Models
{
    public class ShoppingList
    {
        public List<Ingredient> IngredientsToBuy { get; set; } = new List<Ingredient>();
        public List<FoodBase> IngredientsInStock { get; set; } = new List<FoodBase>();
        public bool IsReadyToCook { get; set; }

        public ShoppingList()
        {
            
        }

        
    }
}

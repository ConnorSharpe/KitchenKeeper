using KitchenKeeper.Classes;

namespace KitchenKeeper.Models
{
    public class ShoppingList
    {
        public List<Ingredient> IngredientsToBuy { get; set; } = new List<Ingredient>();
        public List<Food> IngredientsInStock { get; set; } = new List<Food>();
        // Computed property: true when there are no ingredients to buy
        public bool IsReadyToCook => IngredientsToBuy == null || IngredientsToBuy.Count == 0;
    }
}

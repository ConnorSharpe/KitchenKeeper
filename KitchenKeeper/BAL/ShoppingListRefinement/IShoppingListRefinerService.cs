using KitchenKeeper.Classes;
using KitchenKeeper.Models;

namespace KitchenKeeper.BAL.ShoppingListRefinement_BAL
{
    public interface IShoppingListRefinerService
    {
        Task<ShoppingList> GenerateShoppingListFromRecipe(Recipe recipe);
    }
}

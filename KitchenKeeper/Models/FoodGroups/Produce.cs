namespace KitchenKeeper.Classes.FoodGroups
{
    public class Produce: FoodBase
    {
    }

    #region Fruit
    public class Fruit: Produce
    {
    }

    public class Berries: Fruit
    {
        private const int FRIDGE_EXPIRY_DAYS = 5;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    public class Citrus: Fruit
    {
        private const int FRIDGE_EXPIRY_DAYS = 14;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    public class StoneFruit: Fruit
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    public class Melon: Fruit
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    public class TropicalFruit: Fruit
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    #endregion
    #region Vegetable
    public class Vegetable: Produce
    {
    }
    public class LeafyGreen: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
    public class Cruciferous: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
    public class Root: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 14;
        private const int FREEZER_EXPIRY_MONTHS = 6;

    }
    public class Allium: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 30;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
    public class Nightshade: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
    public class Legume: Vegetable
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
    #endregion
}

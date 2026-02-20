namespace KitchenKeeper.Classes.FoodGroups
{
    public class Dairy: FoodBase
    {
        public SubClass SubClass { get; set; }
    }
    public enum SubClass
    {
        Milk,
        Yogurt,
        Butter,
        IceCream
    }

    public class Milk: Dairy
    {
        private const int FRIDGE_EXPIRY_DAYS = 7;
        private const int FREEZER_EXPIRY_MONTH = 7;
    }

    public class Yogurt: Dairy
    {
        private const int FRIDGE_EXPIRY_DAYS = 14;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }

    public class Butter: Dairy
    {
        private const int FRIDGE_EXPIRY_MONTH = 1;
        private const int FREEZER_EXPIRY_YEAR = 1;
    }

    public class IceCream : Dairy
    {
        private const int FREEZER_EXPIRY_YEAR = 1;
    }
}

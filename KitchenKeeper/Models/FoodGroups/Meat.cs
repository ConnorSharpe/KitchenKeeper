namespace KitchenKeeper.Classes.FoodGroups
{
    public class Meat: FoodBase
    {
    }

    public class RedMeat: Meat
    {
        private const int FRIDGE_EXPIRY_DAYS = 5;
        private const int FREEZER_EXPIRY_YEAR = 1;
        private const string FREEZER_EXPIRY_UNIT = "year";
    }

    public class Poultry: Meat
    {
        private const int FRIDGE_EXPIRY_DAYS = 2;
        private const int FREEZER_EXPIRY_YEAR = 1;
        private const string FREEZER_EXPIRY_UNIT = "year";
    }

    public class Fish: Meat
    {
        private const int FRIDGE_EXPIRY_DAYS = 2;
        private const int FREEZER_EXPIRY_MONTHS = 3;

    }

    public class Shellfish: Meat
    {
        private const int FRIDGE_EXPIRY_DAY = 1;
        private const int FREEZER_EXPIRY_MONTHS = 6;
    }
}

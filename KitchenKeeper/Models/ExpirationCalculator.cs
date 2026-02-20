namespace KitchenKeeper.Classes
{
    public class ExpirationCalculator
    {
        public static DateOnly SetDate(DateOnly dateAdded, StorageType storageType)
        {
            DateOnly twoWeeksFromNow = dateAdded.AddDays(14);
            //TODO calculate EXP based on subclass and groups
            return twoWeeksFromNow;
        }
    }
}

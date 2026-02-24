namespace KitchenKeeper.BAL.ExpirationCalculator_BAL
{
    public class ExpirationCalculator
    {
        DateOnly? SetDate(DateOnly dateAdded)
        {
            return (DateOnly?)dateAdded.AddDays(14);
        }
    }
}

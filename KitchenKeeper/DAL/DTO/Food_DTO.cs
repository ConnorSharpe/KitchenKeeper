using KitchenKeeper.Classes;

namespace KitchenKeeper.DAL.DTO
{
    public class Food_DTO
    {
        public int? ID { get; set; }
        public required string Name { get; set; }
        public DateOnly DateAdded { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public required string Storage { get; set; }
        public double Quantity { get; set; }
        public required string UnitOfMeasurement { get; set; }
        public required string Class { get; set; }
        public required string Subclass { get; set; }
        public bool IsCooked { get; set; }
        public bool IsVegetarian { get; set; }
    }
}

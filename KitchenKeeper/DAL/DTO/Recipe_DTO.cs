namespace KitchenKeeper.DAL.DTO
{
    public class Recipe_DTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
    }

    public class RecipeInstructions_DTO
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public int Order { get; set; }
        public required string Content { get; set; }
    }

    public class RecipeIngredients_DTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public int RecipeID { get; set; }
        public double Quantity { get; set; }
        public required string UnitOfMeasurement { get; set; }
    }
}

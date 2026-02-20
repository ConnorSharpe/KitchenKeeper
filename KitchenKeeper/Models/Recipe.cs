namespace KitchenKeeper.Classes
{
    public class Recipe
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
        public required List<Instruction> Instructions { get; set; }

    }

    public class Ingredient
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public int RecipeID { get; set; }
        public double Quantity { get; set; }
        public QuantityType UnitOfMeasurement { get; set; }
    }

    public class Instruction
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public int Order { get; set; }
        public required string Content { get; set; }
    }
}

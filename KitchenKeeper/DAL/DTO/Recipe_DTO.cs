using System.Data;

namespace KitchenKeeper.DAL.DTO
{
    public class Recipe_DTO
    {
        public required Recipe_Details_DTO RecipeDetails { get; set; }
        public required DataTable IngredientsDT { get; set; }
        public required DataTable InstructionsDT { get; set; }
    }

    public class Recipe_Details_DTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
    }

    public class Ingredient_DTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public int RecipeID { get; set; }
        public double Quantity { get; set; }
        public required string UnitOfMeasurement { get; set; }
    }

    public class Instructions_DTO
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public int Order { get; set; }
        public required string Content { get; set; }
    }

    
}

using KitchenKeeper.DAL.DTO;
using System.Runtime.CompilerServices;

namespace KitchenKeeper.Classes
{
    public class FoodBase
    {
        public int? ID { get; set; }
        public required string Name { get; set; }
        public DateOnly DateAdded { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public StorageType Storage { get; set; }
        public double Quantity { get; set; }
        public QuantityType UnitOfMeasurement { get; set; }
        public FoodClass Class { get; set; }
        public FoodSubclass Subclass { get; set; }
        public bool IsCooked { get; set; }
        public bool IsVegetarian { get; set; }

        public virtual DateOnly CalculateExpiration()
        {
            return DateAdded.AddDays(14);
        }
    }

    public enum FoodClass
    {
        Dairy,
        Meat,
        Fruit,
        Vegetable,
        Grain,
        Meal,
        Other
    }

    public enum FoodSubclass
    {
        Dairy_Milk,
        Dairy_Yogurt,
        Dairy_Butter,
        Dairy_IceCream,
        Meat_RedMeat,
        Meat_Poultry,
        Meat_Fish,
        Meat_Shellfish,
        Grain,
        Meal,
        Fruit_Berry,
        Fruit_Citrus,
        Fruit_StoneFruit,
        Fruit_Melon,
        Fruit_TropicalFruit,
        Vegetable_LeafyGreen,
        Vegetable_Cruciferous,
        Vegetable_Root,
        Vegetable_Allium,
        Vegetable_Nightshade,
        Vegetable_Legume
    }

    public enum StorageType
    {
        Shelf,
        Refrigerator,
        Freezer
    }

    public enum QuantityType
    {
        serving,
        gram,
        cup,
        oz,
        fl_oz,
        count
    }
}
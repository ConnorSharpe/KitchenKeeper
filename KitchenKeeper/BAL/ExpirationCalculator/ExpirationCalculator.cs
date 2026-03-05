using KitchenKeeper.Classes;

namespace KitchenKeeper.BAL.ExpirationCalculator_BAL
{
    public class ExpirationCalculator
    {
        private static readonly Dictionary<(FoodSubclass, StorageType), (int, string)> _shelfLifeValues =
            new()
            {
                {(FoodSubclass.Dairy_Milk, StorageType.Refrigerator), (7, "day")},
                {(FoodSubclass.Dairy_Milk, StorageType.Freezer), (3, "month") },
                {(FoodSubclass.Dairy_Yogurt, StorageType.Refrigerator), (14, "day")},
                {(FoodSubclass.Dairy_Yogurt, StorageType.Freezer), (2, "month")},
                {(FoodSubclass.Dairy_Butter, StorageType.Refrigerator), (3, "month")},
                {(FoodSubclass.Dairy_Butter, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Dairy_IceCream, StorageType.Freezer), (4, "month")},
                {(FoodSubclass.Meat_RedMeat, StorageType.Refrigerator), (5, "day")},
                {(FoodSubclass.Meat_RedMeat, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Meat_Poultry, StorageType.Refrigerator), (2, "day")},
                {(FoodSubclass.Meat_Poultry, StorageType.Freezer), (9, "month")},
                {(FoodSubclass.Meat_Fish, StorageType.Refrigerator), (2, "day")},
                {(FoodSubclass.Meat_Fish, StorageType.Freezer), (4, "month")},
                {(FoodSubclass.Meat_Shellfish, StorageType.Refrigerator), (2, "day")},
                {(FoodSubclass.Meat_Shellfish, StorageType.Freezer), (6, "month")},
                {(FoodSubclass.Grain, StorageType.Pantry), (3, "year")},
                {(FoodSubclass.Meal_Vegetarian, StorageType.Refrigerator), (7, "day")},
                {(FoodSubclass.Meal_Vegetarian, StorageType.Freezer), (6, "month")},
                {(FoodSubclass.Meal_Meat, StorageType.Refrigerator), (5, "day")},
                {(FoodSubclass.Meal_Meat, StorageType.Freezer), (6, "month")},
                {(FoodSubclass.Fruit_Berry, StorageType.Refrigerator), (10, "day")},
                {(FoodSubclass.Fruit_Berry, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Fruit_Citrus, StorageType.Pantry), (7, "day")},
                {(FoodSubclass.Fruit_Citrus, StorageType.Refrigerator), (1, "month")},
                {(FoodSubclass.Fruit_Citrus, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Fruit_StoneFruit, StorageType.Pantry), (3, "day")},
                {(FoodSubclass.Fruit_StoneFruit, StorageType.Refrigerator), (7, "day")},
                {(FoodSubclass.Fruit_StoneFruit, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Fruit_Melon, StorageType.Pantry), (4, "day")},
                {(FoodSubclass.Fruit_Melon, StorageType.Refrigerator), (14, "day")},
                {(FoodSubclass.Fruit_Melon, StorageType.Freezer), (8, "month")},
                {(FoodSubclass.Fruit_TropicalFruit, StorageType.Pantry), (3, "day")},
                {(FoodSubclass.Fruit_TropicalFruit, StorageType.Refrigerator), (7, "day")},
                {(FoodSubclass.Fruit_TropicalFruit, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Vegetable_LeafyGreen, StorageType.Refrigerator), ( 10, "day")},
                {(FoodSubclass.Vegetable_LeafyGreen, StorageType.Freezer), ( 8, "month")},
                {(FoodSubclass.Vegetable_Cruciferous, StorageType.Refrigerator), (7, "day")},
                {(FoodSubclass.Vegetable_Cruciferous, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Vegetable_Root, StorageType.Pantry), (20, "day")},
                {(FoodSubclass.Vegetable_Root, StorageType.Refrigerator), (1, "month")},
                {(FoodSubclass.Vegetable_Root, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Vegetable_Allium, StorageType.Pantry), (7, "day")},
                {(FoodSubclass.Vegetable_Allium, StorageType.Refrigerator), (21, "day")},
                {(FoodSubclass.Vegetable_Allium, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Vegetable_Nightshade, StorageType.Pantry), (7, "day")},
                {(FoodSubclass.Vegetable_Nightshade, StorageType.Refrigerator), (14, "day")},
                {(FoodSubclass.Vegetable_Nightshade, StorageType.Freezer), (1, "year")},
                {(FoodSubclass.Vegetable_Legume, StorageType.Pantry), (1, "year") }
            };

        public static DateOnly Calculate(FoodSubclass subclass, StorageType storageType)
        {
            if (_shelfLifeValues.TryGetValue((subclass, storageType), out var shelfLife))
            {
                int duration = shelfLife.Item1;
                string unit = shelfLife.Item2;
                return unit switch
                {
                    "day" => DateOnly.FromDateTime(DateTime.Now).AddDays(duration),
                    "month" => DateOnly.FromDateTime(DateTime.Now).AddMonths(duration),
                    "year" => DateOnly.FromDateTime(DateTime.Now).AddYears(duration)
                };
            }
            throw new Exception("Invalid Storage");
        }
    }
}

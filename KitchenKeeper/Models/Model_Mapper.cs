using KitchenKeeper.Classes;
using KitchenKeeper.DAL.DTO;
using System.Data;

namespace KitchenKeeper.Models
{
    public class Model_Mapper
    {
        public static IEnumerable<Food_DTO> ConvertFoodListToDTOList(IEnumerable<Food> foodList)
        {
            List<Food_DTO> dtoList = new List<Food_DTO>();
            foreach (var food in foodList)
            {
                dtoList.Add(ConvertFoodToDTO(food));
            }
            return dtoList;
        }

        public static Food_DTO ConvertFoodToDTO(Food food)
        {

            Food_DTO food_DTO = new Food_DTO
            {
                ID = food.ID,
                Name = food.Name,
                DateAdded = food.DateAdded,
                ExpirationDate = (DateOnly)food.ExpirationDate,
                Storage = food.Storage.ToString(),
                Quantity = food.Quantity,
                UnitOfMeasurement = food.UnitOfMeasurement.ToString(),
                Class = food.Class.ToString(),
                Subclass = food.Subclass.ToString(),
                IsCooked = food.IsCooked,
                IsVegetarian = food.IsVegetarian
            };

            return food_DTO;
        }

        public static IEnumerable<Food> ConvertDTOListToFoodList(IEnumerable<Food_DTO> dtoList)
        {
            List<Food> foodList = new List<Food>();
            foreach (var dto in dtoList)
            {
                foodList.Add(ConvertDTOToFood(dto));
            }
            return foodList;
        }

        public static Food ConvertDTOToFood(Food_DTO food_DTO)
        {
            Food food = new Food
            {
                ID = food_DTO.ID,
                Name = food_DTO.Name,
                DateAdded = food_DTO.DateAdded,
                ExpirationDate = food_DTO.ExpirationDate,
                Storage = Enum.Parse<StorageType>(food_DTO.Storage),
                Quantity = food_DTO.Quantity,
                UnitOfMeasurement = Enum.Parse<QuantityType>(food_DTO.UnitOfMeasurement),
                Class = Enum.Parse<FoodClass>(food_DTO.Class),
                Subclass = Enum.Parse<FoodSubclass>(food_DTO.Subclass),
                IsCooked = food_DTO.IsCooked,
                IsVegetarian = food_DTO.IsVegetarian
            };
            return food;
        }

        public static Recipe_DTO ConvertRecipeToDTO(Recipe recipe)
        {
            return new Recipe_DTO
            {
                RecipeDetails = new Recipe_Details_DTO
                {
                    ID = recipe.ID,
                    Name = recipe.Name
                },
                IngredientsDT = ConvertIngredientsToDataTable(recipe.Ingredients),
                InstructionsDT = ConvertInstructionsToDataTable(recipe.Instructions)
            };
        }

        public static DataTable ConvertIngredientsToDataTable(List<Ingredient> ingredients)
        {
            DataTable ingredientDT = new DataTable();
            ingredientDT.Columns.Add("Name", typeof(string));
            ingredientDT.Columns.Add("Quantity", typeof(double));
            ingredientDT.Columns.Add("UnitOfMeasurement", typeof(string));
            foreach (var ingredient in ingredients)
            {
                ingredientDT.Rows.Add(ingredient.Name, ingredient.Quantity, ingredient.UnitOfMeasurement.ToString());
            }
            return ingredientDT;
        }

        public static DataTable ConvertInstructionsToDataTable(List<Instruction> instructions)
        {
            DataTable instructionDT = new DataTable();
            instructionDT.Columns.Add("Order", typeof(int));
            instructionDT.Columns.Add("Content", typeof(string));
            foreach (var instruction in instructions)
            {
                instructionDT.Rows.Add(instruction.Order, instruction.Content);
            }
            return instructionDT;
        }

        public static Ingredient_DTO ConvertIngredientToDTO(Ingredient ingredient)
        {
            return new Ingredient_DTO
            {
                ID = ingredient.ID,
                Name = ingredient.Name,
                RecipeID = ingredient.RecipeID,
                Quantity = ingredient.Quantity,
                UnitOfMeasurement = ingredient.UnitOfMeasurement.ToString()
            };
        }

        public static Instruction_DTO ConvertInstructionToDTO(Instruction instruction)
        {
            return new Instruction_DTO
            {
                ID = instruction.ID,
                RecipeID = instruction.RecipeID,
                Order = instruction.Order,
                Content = instruction.Content,
            };
        }

        public static IEnumerable<Recipe> ConvertDTOListToRecipeList(IEnumerable<Recipe_DTO> dtoList)
        {
            List<Recipe> recipeList = new List<Recipe>();
            foreach (var dto in dtoList)
            {
                recipeList.Add(ConvertDTOToRecipe(dto));
            }
            return recipeList;
        }

        public static Recipe ConvertDTOToRecipe(Recipe_DTO recipe_DTO)
        {
            return new Recipe
            {
                ID = recipe_DTO.RecipeDetails.ID,
                Name = recipe_DTO.RecipeDetails.Name,
                Ingredients = ConvertDTOToIngredientList(recipe_DTO.IngredientsDT),
                Instructions = ConvertDTOToInstructionList(recipe_DTO.InstructionsDT)
            };
        }

        public static List<Ingredient> ConvertDTOToIngredientList(DataTable ingredientDT)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (DataRow row in ingredientDT.Rows)
            {
                Ingredient ingredient = new Ingredient
                {
                    Name = row["Name"].ToString(),
                    Quantity = Convert.ToDouble(row["Quantity"]),
                    UnitOfMeasurement = Enum.Parse<QuantityType>(row["UnitOfMeasurement"].ToString())
                };
                ingredients.Add(ingredient);
            }
            return ingredients;
        }

        public static Ingredient ConvertIngredientDataRowToIngredient(DataRow ingredientRow)
        {
            return new Ingredient
            {
                ID = Convert.ToInt32(ingredientRow["ID"]),
                Name = ingredientRow["Name"].ToString(),
                RecipeID = Convert.ToInt32(ingredientRow["RecipeID"]),
                Quantity = Convert.ToDouble(ingredientRow["Quantity"]),
                UnitOfMeasurement = Enum.Parse<QuantityType>(ingredientRow["UnitOfMeasurement"].ToString())
            };
        }

        public static List<Instruction> ConvertDTOToInstructionList(DataTable instructionDT)
        {
            List<Instruction> instructions = new List<Instruction>();
            foreach (DataRow row in instructionDT.Rows)
            {
                Instruction instruction = new Instruction
                {
                    Order = Convert.ToInt32(row["Order"]),
                    Content = row["Content"].ToString()
                };
                instructions.Add(instruction);
            }
            return instructions;
        }

        public static Instruction ConvertInstructionDataRowToInstruction(DataRow instructionRow)
        {
            return new Instruction
            {
                ID = Convert.ToInt32(instructionRow["ID"]),
                RecipeID = Convert.ToInt32(instructionRow["RecipeID"]),
                Order = Convert.ToInt32(instructionRow["Order"]),
                Content = instructionRow["Content"].ToString()
            };
        }
    }
}

-- User-defined table type for Ingredients
IF TYPE_ID(N'dbo.IngredientType') IS NULL
BEGIN
    CREATE TYPE dbo.IngredientType AS TABLE
    (
        ID INT NULL,
        Name NVARCHAR(256) NOT NULL,
        RecipeID INT NULL,
        Quantity FLOAT NOT NULL,
        UnitOfMeasurement NVARCHAR(64) NOT NULL
    );
END
GO

-- User-defined table type for Instructions
IF TYPE_ID(N'dbo.InstructionType') IS NULL
BEGIN
    CREATE TYPE dbo.InstructionType AS TABLE
    (
        ID INT NULL,
        RecipeID INT NULL,
        [Order] INT NOT NULL,
        Content NVARCHAR(MAX) NOT NULL
    );
END
GO

-- Stored procedure to update a recipe and its child collections (ingredients, instructions)
-- Expected usage: pass Recipe details and two table-valued parameters (ingredients, instructions)
CREATE OR ALTER PROCEDURE dbo.RECIPE_UPDATE
    @RecipeID INT,
    @Name NVARCHAR(256),
    @Ingredients dbo.IngredientType READONLY,
    @Instructions dbo.InstructionType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Update recipe header
        UPDATE dbo.Recipes
        SET Name = @Name,
            ModifiedDate = SYSUTCDATETIME()
        WHERE ID = @RecipeID;

        -- If recipe row does not exist, raise error
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Recipe with ID %d not found.', 16, 1, @RecipeID);
            ROLLBACK TRANSACTION;
            RETURN -1;
        END

        -- MERGE Ingredients: update existing, insert new (where ID is NULL or 0)
        ;WITH SrcIngredients AS (
            SELECT ID, Name, Quantity, UnitOfMeasurement
            FROM @Ingredients
        )
        MERGE INTO dbo.Ingredients AS T
        USING (
            SELECT ID, Name, Quantity, UnitOfMeasurement
            FROM SrcIngredients
        ) AS S
        ON T.ID = S.ID AND T.RecipeID = @RecipeID AND S.ID IS NOT NULL AND S.ID <> 0
        WHEN MATCHED THEN
            UPDATE SET
                T.Name = S.Name,
                T.Quantity = S.Quantity,
                T.UnitOfMeasurement = S.UnitOfMeasurement
        WHEN NOT MATCHED BY TARGET AND (S.ID IS NULL OR S.ID = 0) THEN
            INSERT (Name, RecipeID, Quantity, UnitOfMeasurement)
            VALUES (S.Name, @RecipeID, S.Quantity, S.UnitOfMeasurement)
        ;

        -- Remove any ingredient rows for this recipe that were not included in the incoming TVP (IDs > 0)
        DELETE FROM dbo.Ingredients
        WHERE RecipeID = @RecipeID
          AND ID NOT IN (SELECT ID FROM @Ingredients WHERE ID IS NOT NULL AND ID <> 0);

        -- MERGE Instructions: update existing, insert new (ID NULL or 0), and set ordering
        ;WITH SrcInstructions AS (
            SELECT ID, [Order], Content
            FROM @Instructions
        )
        MERGE INTO dbo.Instructions AS T
        USING (
            SELECT ID, [Order], Content
            FROM SrcInstructions
        ) AS S
        ON T.ID = S.ID AND T.RecipeID = @RecipeID AND S.ID IS NOT NULL AND S.ID <> 0
        WHEN MATCHED THEN
            UPDATE SET
                T.[Order] = S.[Order],
                T.Content = S.Content
        WHEN NOT MATCHED BY TARGET AND (S.ID IS NULL OR S.ID = 0) THEN
            INSERT (RecipeID, [Order], Content)
            VALUES (@RecipeID, S.[Order], S.Content)
        ;

        -- Remove instruction rows not present in incoming TVP (IDs > 0)
        DELETE FROM dbo.Instructions
        WHERE RecipeID = @RecipeID
          AND ID NOT IN (SELECT ID FROM @Instructions WHERE ID IS NOT NULL AND ID <> 0);

        COMMIT TRANSACTION;
        RETURN 1;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
        RETURN -1;
    END CATCH
END
GO

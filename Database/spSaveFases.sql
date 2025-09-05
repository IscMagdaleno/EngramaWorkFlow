IF OBJECT_ID('spSaveFases') IS NULL EXEC ('CREATE PROCEDURE spSaveFases AS SET NOCOUNT ON;') 
GO
ALTER PROCEDURE spSaveFases (
    @FasesList dbo.DTFases READONLY  -- Parámetro tipo tabla para múltiples fases
)
AS BEGIN
    DECLARE @vchError VARCHAR(200) = '';
    DECLARE @Result TABLE (bResult BIT DEFAULT(1), vchMessage VARCHAR(500) DEFAULT(''), iInsertedCount INT DEFAULT(0));

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Inserta todas las fases recibidas en la tabla Fases
        INSERT INTO dbo.Fases (
            iPlanID,
            iNumeroFase,
            nvchTitulo,
            nvchDescripcion,
            nvchEstado,
            dtFechaCreacion
        )
        SELECT 
            FL.iPlanID,
            FL.iNumeroFase,
            FL.nvchTitulo,
            FL.nvchDescripcion,
            'Pendiente',  -- Estado predeterminado
            GETDATE()
        FROM @FasesList FL;

        DECLARE @InsertedCount INT = @@ROWCOUNT;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT @vchError = CONCAT('spSaveFases: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
    END CATCH

    IF LEN(@vchError) > 0
    BEGIN
        INSERT INTO @Result (bResult, vchMessage) VALUES (0, @vchError);
    END
    ELSE
    BEGIN
        INSERT INTO @Result (bResult, vchMessage, iInsertedCount) VALUES (1, 'Fases insertadas correctamente', @InsertedCount);
    END

    SELECT * FROM @Result;
    SET NOCOUNT OFF;
END;
GO
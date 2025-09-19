IF OBJECT_ID( 'spSaveRespuestas' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveRespuestas AS SET NOCOUNT ON;') 
GO 
ALTER PROCEDURE dbo.spSaveRespuestas
(
    @RespuestasList dbo.DTRespuestas READONLY   -- Parámetro tipo tabla para múltiples registros
)
AS
BEGIN
    -- Tabla local para resultado estándar
    DECLARE @Result TABLE (
        bResult BIT DEFAULT(1),
        vchMessage VARCHAR(500) DEFAULT(''),
        iInsertedCount INT DEFAULT(0)
    );

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @vchError VARCHAR(200) = '';
    DECLARE @InsertedCount INT = 0;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Inserta todos los registros recibidos en la tabla Respuestas
        INSERT INTO dbo.Respuestas
        (
            iPlanID,
            nvchPregunta,
            nvchRespuesta,
            dtFechaCreacion
        )
        SELECT 
            RL.iPlanID,
            RL.nvchPregunta,
            RL.nvchRespuesta,
            RL.dtFechaCreacion
        FROM
            @RespuestasList RL;

        SET @InsertedCount = @@ROWCOUNT;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        SELECT @vchError = CONCAT(
            'spSaveRespuestas: ', ERROR_MESSAGE(), ' ',
            ERROR_PROCEDURE(), ' ',
            ERROR_LINE()
        );
    END CATCH

    -- Devuelve el resultado de la operación
    IF LEN(@vchError) > 0
    BEGIN
        INSERT INTO @Result (bResult, vchMessage)
        VALUES (0, @vchError);
    END
    ELSE
    BEGIN
        INSERT INTO @Result (bResult, vchMessage, iInsertedCount)
        VALUES (1, 'Registros insertados correctamente', @InsertedCount);
    END

    SELECT * FROM @Result;
    SET NOCOUNT OFF;
END
GO



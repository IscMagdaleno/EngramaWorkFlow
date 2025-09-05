IF OBJECT_ID('spGetRespuestasByPlan') IS NULL EXEC ('CREATE PROCEDURE spGetRespuestasByPlan AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE spGetRespuestasByPlan (@iPlanID INT)
AS
BEGIN
    DECLARE @vchError VARCHAR(200) = '';
    DECLARE @Result TABLE (bResult BIT DEFAULT(1), vchMessage VARCHAR(500) DEFAULT(''), nvchTitulo NVARCHAR(200), RespuestasJson NVARCHAR(MAX)); -- Cambia a JSON

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Obtener título del plan
        DECLARE @nvchTitulo NVARCHAR(200);
        SELECT @nvchTitulo = nvchTitulo 
        FROM dbo.Planes 
        WHERE iPlanID = @iPlanID;

        IF @nvchTitulo IS NULL
        BEGIN
            INSERT INTO @Result (bResult, vchMessage)
            VALUES (0, 'Plan no encontrado');
            SELECT * FROM @Result;
            RETURN;
        END

        -- Obtener respuestas en una tabla variable
        DECLARE @Respuestas AS dbo.DTRespuestas;
        INSERT INTO @Respuestas (iRespuestaID, iPlanID, nvchPregunta, nvchRespuesta, dtFechaCreacion)
        SELECT iRespuestaID, iPlanID, nvchPregunta, nvchRespuesta, dtFechaCreacion
        FROM dbo.Respuestas 
        WHERE iPlanID = @iPlanID;

        -- Serializar respuestas como JSON
        DECLARE @RespuestasJson NVARCHAR(MAX);
        SET @RespuestasJson = (SELECT * FROM @Respuestas FOR JSON PATH);

        -- Devolver resultado
        INSERT INTO @Result (bResult, vchMessage, nvchTitulo, RespuestasJson)
        VALUES (1, '', @nvchTitulo, @RespuestasJson);

    END TRY
    BEGIN CATCH
        SELECT @vchError = CONCAT('spGetRespuestasByPlan: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
        INSERT INTO @Result (bResult, vchMessage)
        VALUES (0, @vchError);
    END CATCH

    SELECT * FROM @Result;
    SET NOCOUNT OFF;
END;
GO
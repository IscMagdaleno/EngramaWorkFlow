IF OBJECT_ID('dbo.spSaveFase', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spSaveFase
GO

CREATE PROCEDURE dbo.spSaveFase
    @iIdFase            INT,
    @iIdProyecto        INT,
    @smNumeroSecuencia  SMALLINT,
    @nvchTitulo         NVARCHAR(510),
    @nvchDescripcion    NVARCHAR(MAX),
    @dtCreadoEn         DATETIME,
    @dtActualizadoEn    DATETIME,
    @LstPasos            DTPaso READONLY -- parámetro tabla para los pasos
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @vchError VARCHAR(200) = '';

    -- Tabla resultado para mensajes y datos de salida
    DECLARE @Result TABLE (
        bResult    BIT       DEFAULT(1),
        vchMessage VARCHAR(500) DEFAULT(''),
        iIdFase    INT       DEFAULT(-1)
    );

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar o actualizar la fase
        IF (@iIdFase IS NULL OR @iIdFase <= 0)
        BEGIN
            INSERT INTO dbo.Fase (
                iIdProyecto,
                smNumeroSecuencia,
                nvchTitulo,
                nvchDescripcion,
                dtCreadoEn,
                dtActualizadoEn
            )
            VALUES (
                @iIdProyecto,
                @smNumeroSecuencia,
                @nvchTitulo,
                @nvchDescripcion,
                @dtCreadoEn,
                @dtActualizadoEn
            );

            -- Obtiene el nuevo ID generado
            SET @iIdFase = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE dbo.Fase WITH (ROWLOCK)
            SET
                iIdProyecto = @iIdProyecto,
                smNumeroSecuencia = @smNumeroSecuencia,
                nvchTitulo = @nvchTitulo,
                nvchDescripcion = @nvchDescripcion,
                dtCreadoEn = @dtCreadoEn,
                dtActualizadoEn = @dtActualizadoEn
            WHERE iIdFase = @iIdFase;
        END

        -- Insertar los pasos relacionados a la fase (si hay datos en el parámetro tabla)
        IF EXISTS (SELECT 1 FROM @LstPasos)
        BEGIN
            INSERT INTO dbo.Paso (
                iIdFase,
                smNumeroSecuencia,
                nvchDescripcion,
                nvchProposito,
                nvchCaracteristicas,
                nvchEnfoque,
                dtCreadoEn,
                dtActualizadoEn
            )
            SELECT
                @iIdFase,                        -- Relaciona cada paso con la fase actual
                p.smNumeroSecuencia,
                p.nvchDescripcion,
                p.nvchProposito,
                p.nvchCaracteristicas,
                p.nvchEnfoque,
                p.dtCreadoEn,
                p.dtActualizadoEn
            FROM
                @LstPasos AS p;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        SELECT @vchError = CONCAT('spSaveFase: ', ERROR_MESSAGE(), ' Procedure: ', ERROR_PROCEDURE(), ' Line: ', ERROR_LINE());
        PRINT @vchError;
    END CATCH

    -- Resultado final
    IF LEN(@vchError) > 0
    BEGIN
        INSERT INTO @Result (bResult, vchMessage)
        VALUES (0, @vchError);
    END
    ELSE
    BEGIN
        INSERT INTO @Result (bResult, vchMessage, iIdFase)
        VALUES (1, '', @iIdFase);
    END

    SELECT * FROM @Result;

    SET NOCOUNT OFF;
END
GO
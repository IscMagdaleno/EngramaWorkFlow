ALTER PROCEDURE dbo.spSaveModulo
(
    @iIdModulo INT,
    @iIdPlanTrabajo INT,
    @vchTitulo VARCHAR(100),
    @nvchProposito NVARCHAR(2000),
    @LstFuncionalidades dbo.DTFuncionalidad READONLY
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @vchError VARCHAR(200) = '';

    DECLARE @Result AS TABLE
    (
        bResult BIT DEFAULT(1),
        vchMessage VARCHAR(500) DEFAULT(''),
        iIdModulo INT DEFAULT(-1)
    );

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar o actualizar el módulo
        IF (@iIdModulo IS NULL OR @iIdModulo <= 0)
        BEGIN
            INSERT INTO dbo.Modulo (iIdPlanTrabajo, vchTitulo, nvchProposito)
            VALUES (@iIdPlanTrabajo, @vchTitulo, @nvchProposito);

            SET @iIdModulo = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE dbo.Modulo WITH(ROWLOCK)
            SET
                iIdPlanTrabajo = @iIdPlanTrabajo,
                vchTitulo = @vchTitulo,
                nvchProposito = @nvchProposito
            WHERE iIdModulo = @iIdModulo;
        END

        -- Sincronizar funcionalidades (INSERTAR, ACTUALIZAR, ELIMINAR)
        -- Actualizar funcionalidades existentes
        UPDATE F
        SET
            F.nvchDescripcion = L.nvchDescripcion,
            F.nvchEntidades = L.nvchEntidades,
            F.nvchInteracciones = L.nvchInteracciones,
            F.nvchTecnico = L.nvchTecnico,
            F.nvchConsideraciones = L.nvchConsideraciones
        FROM dbo.Funcionalidad F
        INNER JOIN @LstFuncionalidades L
            ON F.iIdFuncionalidad = L.iIdFuncionalidad
        WHERE F.iIdModulo = @iIdModulo;

        -- Insertar nuevas funcionalidades
        INSERT INTO dbo.Funcionalidad
        (
            iIdModulo,
            nvchDescripcion,
            nvchEntidades,
            nvchInteracciones,
            nvchTecnico,
            nvchConsideraciones
        )
        SELECT
            @iIdModulo,
            L.nvchDescripcion,
            L.nvchEntidades,
            L.nvchInteracciones,
            L.nvchTecnico,
            L.nvchConsideraciones
        FROM @LstFuncionalidades L
        LEFT JOIN dbo.Funcionalidad F
            ON L.iIdFuncionalidad = F.iIdFuncionalidad AND F.iIdModulo = @iIdModulo
        WHERE F.iIdFuncionalidad IS NULL;


        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        SET @vchError = CONCAT('spSaveModulo: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
        PRINT @vchError;
    END CATCH

    IF LEN(@vchError) > 0
    BEGIN
        INSERT INTO @Result (bResult, vchMessage)
        VALUES (0, @vchError);
    END
    ELSE
    BEGIN
        INSERT INTO @Result (bResult, vchMessage, iIdModulo)
        VALUES (1, '', @iIdModulo);
    END

    SELECT *
    FROM @Result;

    SET NOCOUNT OFF;
END
GO
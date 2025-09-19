IF OBJECT_ID('spSaveFuncionalidades') IS NULL EXEC ('CREATE PROCEDURE spSaveFuncionalidades AS SET NOCOUNT ON;')
  GO
  ALTER PROCEDURE spSaveFuncionalidades (
    @iIdFuncionalidad INT,
    @iIdPlanTrabajo INT,
    @vchNombre VARCHAR (100),
    @nvchDescripcion NVARCHAR (2000),
    @nvchProceso NVARCHAR (2000),
    @nvchDetalle NVARCHAR (4000),
    @nvchAccionUsuario NVARCHAR (2000),
    @vchComponentes VARCHAR (1000),
    @nvchDatosMovidos NVARCHAR (2000),
    @vchEstatus VARCHAR (50)
  ) AS
BEGIN
  DECLARE
  @vchError VARCHAR (200) = '';
  DECLARE
  @Result AS TABLE (bResult BIT DEFAULT (1), vchMessage VARCHAR (500) DEFAULT (''), iIdFuncionalidad INT DEFAULT (- 1));
  SET NOCOUNT ON
  SET XACT_ABORT ON;
  BEGIN
    TRY
    BEGIN TRANSACTION
    IF (@iIdFuncionalidad <= 0)
    BEGIN
      INSERT INTO dbo.Funcionalidades (iIdPlanTrabajo, vchNombre, nvchDescripcion, nvchProceso, nvchDetalle, nvchAccionUsuario, vchComponentes, nvchDatosMovidos, vchEstatus)
      VALUES
      (@iIdPlanTrabajo,		@vchNombre,		@nvchDescripcion, @nvchProceso,		@nvchDetalle,		@nvchAccionUsuario, @vchComponentes,		@nvchDatosMovidos,		@vchEstatus)
      SET @iIdFuncionalidad = @@IDENTITY
    END
  ELSE
    BEGIN
      UPDATE dbo.Funcionalidades WITH (ROWLOCK)
      SET iIdPlanTrabajo = @iIdPlanTrabajo,
      vchNombre = @vchNombre,
      nvchDescripcion = @nvchDescripcion,
      nvchProceso = @nvchProceso,
      nvchDetalle = @nvchDetalle,
      nvchAccionUsuario = @nvchAccionUsuario,
      vchComponentes = @vchComponentes,
      nvchDatosMovidos = @nvchDatosMovidos,
      vchEstatus = @vchEstatus
      WHERE
        iIdFuncionalidad = @iIdFuncionalidad;
    END COMMIT TRANSACTION;
  END TRY
  BEGIN
    CATCH ROLLBACK TRANSACTION;
    SELECT
      @vchError = CONCAT ('spSaveFuncionalidades: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
    PRINT @vchError;
  END CATCH
  IF LEN(@vchError) > 0
  BEGIN
    INSERT INTO @Result (bResult, vchMessage)
    VALUES
    (0,@vchError);
  END
ELSE
  BEGIN
    INSERT INTO @Result (bResult, vchMessage, iIdFuncionalidad)
    VALUES
    (1, '',@iIdFuncionalidad);
  END;
  SELECT
    *
  FROM
    @Result;
  SET NOCOUNT OFF;
END;
GO
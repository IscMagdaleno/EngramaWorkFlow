IF OBJECT_ID('spSavePlanTrabajo') IS NULL EXEC ('CREATE PROCEDURE spSavePlanTrabajo AS SET NOCOUNT ON;')
  GO
  ALTER PROCEDURE spSavePlanTrabajo (@iIdPlanTrabajo INT, @vchNombre VARCHAR (100), @nvchDescripcion NVARCHAR (1000), @vchEstatus VARCHAR (50)) AS
BEGIN
  DECLARE
  @vchError VARCHAR (200) = '';
  DECLARE
  @Result AS TABLE (bResult BIT DEFAULT (1), vchMessage VARCHAR (500) DEFAULT (''), iIdPlanTrabajo INT DEFAULT (- 1));
  SET NOCOUNT ON
  SET XACT_ABORT ON;
  BEGIN
    TRY
    BEGIN TRANSACTION
    IF (@iIdPlanTrabajo <= 0)
    BEGIN
      INSERT INTO dbo.PlanTrabajo (vchNombre, nvchDescripcion, vchEstatus)
      VALUES
      (@vchNombre,		@nvchDescripcion,		'Proceso')
      SET @iIdPlanTrabajo = @@IDENTITY
    END
  ELSE
    BEGIN
      UPDATE dbo.PlanTrabajo WITH (ROWLOCK)
      SET vchNombre = @vchNombre,
      nvchDescripcion = @nvchDescripcion,
      vchEstatus = @vchEstatus
      WHERE
        iIdPlanTrabajo = @iIdPlanTrabajo;
    END COMMIT TRANSACTION;
  END TRY
  BEGIN
    CATCH ROLLBACK TRANSACTION;
    SELECT
      @vchError = CONCAT ('spSavePlanTrabajo: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
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
    INSERT INTO @Result (bResult, vchMessage, iIdPlanTrabajo)
    VALUES
    (1, '',@iIdPlanTrabajo);
  END;
  SELECT
    *
  FROM
    @Result;
  SET NOCOUNT OFF;
END;
GO
IF OBJECT_ID('spGetFuncionalidades') IS NULL EXEC ('CREATE PROCEDURE spGetFuncionalidades AS SET NOCOUNT ON;')
  GO
  ALTER PROCEDURE spGetFuncionalidades (@iIdPlanTrabajo INT) AS
BEGIN
  CREATE TABLE #Result (
    bResult BIT DEFAULT (1),
    vchMessage VARCHAR (500) DEFAULT (''),
    iIdFuncionalidad INT DEFAULT (- 1),
    iIdPlanTrabajo INT DEFAULT (- 1),
    vchNombre VARCHAR (100) DEFAULT (''),
    nvchDescripcion NVARCHAR (2000) DEFAULT (''),
    nvchProceso NVARCHAR (2000) DEFAULT (''),
    vchComponentes VARCHAR (1000) DEFAULT (''),
    nvchDatosMovidos NVARCHAR (2000) DEFAULT (''),
    vchEstatus VARCHAR (50) DEFAULT (''),
  );
  SET NOCOUNT ON
  BEGIN
    TRY INSERT INTO #Result (iIdFuncionalidad, iIdPlanTrabajo, vchNombre, nvchDescripcion, nvchProceso, vchComponentes, nvchDatosMovidos, vchEstatus) SELECT
      F.iIdFuncionalidad,
      F.iIdPlanTrabajo,
      F.vchNombre,
      F.nvchDescripcion,
      F.nvchProceso,
      F.vchComponentes,
      F.nvchDatosMovidos,
      F.vchEstatus
    FROM
      dbo.Funcionalidades F WITH (NOLOCK)
      WHERE F.iIdPlanTrabajo = @iIdPlanTrabajo
      
      
      IF NOT EXISTS (SELECT 1 FROM #Result)
      BEGIN
        INSERT INTO #Result (bResult, vchMessage) SELECT
          0,
          'No register found.';
      END
  END TRY
  BEGIN
    CATCH INSERT INTO #Result (bResult, vchMessage) SELECT
      0,
      CONCAT (ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
    PRINT CONCAT (ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
  END CATCH SELECT
    *
  FROM
    #Result;
  DROP TABLE #Result;
END
GO

EXEC spGetFuncionalidades 1
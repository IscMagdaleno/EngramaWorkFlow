IF OBJECT_ID( 'spGetChat' ) IS NULL
	EXEC ('CREATE PROCEDURE spGetChat AS SET NOCOUNT ON;') 
GO 
ALTER PROCEDURE spGetChat (
@iIdFuncionalidad INT 
) 
AS 
BEGIN 


CREATE TABLE #Result (
	bResult BIT DEFAULT (1),
	vchMessage VARCHAR(500) DEFAULT(''),
	 iIdChat INT DEFAULT( -1 ),
	 iIdFuncionalidad INT DEFAULT( -1 ),
	 dtFechaCreacion DATETIME DEFAULT( '1900-01-01' ),
	 nvchNombre NVARCHAR (200)  DEFAULT( '' ),
	 bActivo BIT DEFAULT( 0 ),
);

SET NOCOUNT ON

	BEGIN TRY

	INSERT INTO  #Result
	 ( 

		iIdChat, 			iIdFuncionalidad, 			dtFechaCreacion, 	
		nvchNombre, 			bActivo 			)
		SELECT 
		 C.iIdChat, 			 C.iIdFuncionalidad, 			 C.dtFechaCreacion, 	
				 C.nvchNombre, 			 C.bActivo 	FROM
		 dbo.Chat C  WITH(NOLOCK)  
WHERE C.iIdFuncionalidad =@iIdFuncionalidad

		IF NOT EXISTS (SELECT 1 FROM #Result)
			BEGIN
				INSERT INTO #Result (bResult, vchMessage)
				SELECT 0, 'No register found.';
			END
	END TRY
	BEGIN CATCH
		INSERT INTO #Result (bResult, vchMessage)
		SELECT 0, CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
		PRINT CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
	END CATCH
	SELECT * FROM #Result;
	DROP TABLE #Result;
	END
GO

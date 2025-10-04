
IF OBJECT_ID( 'spSaveProyecto' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveProyecto AS SET NOCOUNT ON;') 
GO 
ALTER PROCEDURE spSaveProyecto (
@iIdProyecto INT,
@iIdPlanTrabajo INT, 
@nvchNombre NVARCHAR (510) , 
@nvchDescripcion NVARCHAR (MAX)  
) 
AS 
BEGIN 

DECLARE @vchError VARCHAR(200) = '';

DECLARE @Result AS TABLE (
	bResult BIT DEFAULT(1),
	vchMessage VARCHAR(500) DEFAULT(''),
	iIdProyecto INT DEFAULT( -1 ) 
	);

SET NOCOUNT ON
SET XACT_ABORT ON;

BEGIN TRY

BEGIN TRANSACTION

IF  ( @iIdProyecto <= 0) 
BEGIN 
	INSERT INTO dbo.Proyecto
	 ( 

		nvchNombre, 			nvchDescripcion, 			dtCreadoEn, 	
		dtActualizadoEn 	,iIdPlanTrabajo
	)
	VALUES 
	(
		@nvchNombre,		@nvchDescripcion,		GETDATE(),
		GETDATE(), @iIdPlanTrabajo
	)
		 SET @iIdProyecto = @@IDENTITY
END
ELSE
BEGIN
	UPDATE  dbo.Proyecto WITH(ROWLOCK) SET
		 nvchNombre = @nvchNombre, 
		 nvchDescripcion = @nvchDescripcion, 
		 dtActualizadoEn = GETDATE() 

	 WHERE  iIdProyecto  = @iIdProyecto;

END
		COMMIT TRANSACTION ;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION ;

		SELECT @vchError = CONCAT( 'spSaveProyecto: ', ERROR_MESSAGE( ), ' ', ERROR_PROCEDURE( ), ' ', ERROR_LINE( ) );
		PRINT @vchError;
	END CATCH

	IF LEN( @vchError ) > 0
	BEGIN
		INSERT INTO @Result
		(
			bResult,vchMessage
		)
		VALUES
		(
			0,@vchError
		);
	END
	ELSE
	BEGIN
		INSERT INTO @Result
		(
			bResult,vchMessage,iIdProyecto
		)
		VALUES
		(
			1,'',@iIdProyecto
		);
	END;

	SELECT *
	FROM
		@Result;
	SET NOCOUNT OFF;
END;
GO 



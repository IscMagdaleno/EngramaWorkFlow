IF OBJECT_ID( 'spSaveMensaje' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveMensaje AS SET NOCOUNT ON;') 
GO 
ALTER PROCEDURE spSaveMensaje (
@iIdMensaje INT, 
@iIdChat INT, 
@iOrden INT, 
@nvchRol NVARCHAR (40) , 
@nvchContenido NVARCHAR (MAX) , 
@dtFecha DATETIME 
) 
AS 
BEGIN 

DECLARE @vchError VARCHAR(200) = '';

DECLARE @Result AS TABLE (
	bResult BIT DEFAULT(1),
	vchMessage VARCHAR(500) DEFAULT(''),
	iIdMensaje INT DEFAULT( -1 ) 
	);

SET NOCOUNT ON
SET XACT_ABORT ON;

BEGIN TRY

BEGIN TRANSACTION

IF  ( @iIdMensaje <= 0) 
BEGIN 
	INSERT INTO dbo.Mensaje
	 ( 

		iIdChat, 			iOrden, 			nvchRol, 	
		nvchContenido, 			dtFecha 	
	)
	VALUES 
	(
		@iIdChat,		@iOrden,		@nvchRol,
		@nvchContenido,		@dtFecha
	)
		 SET @iIdMensaje = @@IDENTITY
END
ELSE
BEGIN
	UPDATE  dbo.Mensaje WITH(ROWLOCK) SET
		 iIdChat = @iIdChat, 
		 iOrden = @iOrden, 
		 nvchRol = @nvchRol, 
		 nvchContenido = @nvchContenido, 
		 dtFecha = @dtFecha 

	 WHERE  iIdMensaje  = @iIdMensaje;

END
		COMMIT TRANSACTION ;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION ;

		SELECT @vchError = CONCAT( 'spSaveMensaje: ', ERROR_MESSAGE( ), ' ', ERROR_PROCEDURE( ), ' ', ERROR_LINE( ) );
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
			bResult,vchMessage,iIdMensaje
		)
		VALUES
		(
			1,'',@iIdMensaje
		);
	END;

	SELECT *
	FROM
		@Result;
	SET NOCOUNT OFF;
END;
GO 



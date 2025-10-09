IF OBJECT_ID( 'spSaveChat' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveChat AS SET NOCOUNT ON;') 
GO 
ALTER PROCEDURE spSaveChat (
@iIdChat INT, 
@iIdFase INT, 
@dtFechaCreacion DATETIME, 
@nvchNombre NVARCHAR (200) , 
@bActivo BIT 
) 
AS 
BEGIN 

DECLARE @vchError VARCHAR(200) = '';

DECLARE @Result AS TABLE (
	bResult BIT DEFAULT(1),
	vchMessage VARCHAR(500) DEFAULT(''),
	iIdChat INT DEFAULT( -1 ) 
	);

SET NOCOUNT ON
SET XACT_ABORT ON;

BEGIN TRY

BEGIN TRANSACTION

IF  ( @iIdChat <= 0) 
BEGIN 
	INSERT INTO dbo.Chat
	 ( 

		iIdFase, 			dtFechaCreacion, 			nvchNombre, 	
		bActivo 	
	)
	VALUES 
	(
		@iIdFase,		GETDATE(),		@nvchNombre,
		1
	)
		 SET @iIdChat = @@IDENTITY
END
ELSE
BEGIN
	UPDATE  dbo.Chat WITH(ROWLOCK) SET
		 nvchNombre = @nvchNombre, 
		 bActivo = @bActivo 

	 WHERE  iIdChat  = @iIdChat;

END
		COMMIT TRANSACTION ;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION ;

		SELECT @vchError = CONCAT( 'spSaveChat: ', ERROR_MESSAGE( ), ' ', ERROR_PROCEDURE( ), ' ', ERROR_LINE( ) );
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
			bResult,vchMessage,iIdChat
		)
		VALUES
		(
			1,'',@iIdChat
		);
	END;

	SELECT *
	FROM
		@Result;
	SET NOCOUNT OFF;
END;
GO 



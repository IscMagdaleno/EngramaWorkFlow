IF OBJECT_ID('spSavePlanes') IS NULL EXEC ('CREATE PROCEDURE spSavePlanes AS SET NOCOUNT ON;') 
GO
  ALTER PROCEDURE spSavePlanes (
    @iPlanID INT, 
    @iUsuarioID INT, 
    @nvchTitulo NVARCHAR (400), 
    @nvchDescripcion NVARCHAR (2000), 
  @nvchEstado NVARCHAR (40))
AS BEGIN
    DECLARE
      @vchError VARCHAR (200) = '';
    DECLARE
      @Result AS TABLE (bResult BIT DEFAULT (1), vchMessage VARCHAR (500) DEFAULT (''), iPlanID INT DEFAULT (- 1));
    
    SET NOCOUNT ON 
    SET XACT_ABORT ON;
    BEGIN
        TRY BEGIN TRANSACTION
          IF
            (@iPlanID <= 0) BEGIN
              INSERT INTO dbo.Planes (iUsuarioID, nvchTitulo, nvchDescripcion, dtFechaCreacion, nvchEstado)
              VALUES
                (@iUsuarioID,		@nvchTitulo,		@nvchDescripcion,
                GETDATE(),		@nvchEstado) 
                SET @iPlanID = @@IDENTITY 
              END ELSE BEGIN
                UPDATE dbo.Planes WITH (ROWLOCK) 
                SET 
                nvchTitulo = @nvchTitulo,
                nvchDescripcion = @nvchDescripcion,
                nvchEstado = @nvchEstado 
              WHERE
                iPlanID = @iPlanID;
              
            END COMMIT TRANSACTION;
          
        END TRY BEGIN
  CATCH ROLLBACK TRANSACTION;
SELECT
  @vchError = CONCAT ('spSavePlanes: ', ERROR_MESSAGE(), ' ', ERROR_PROCEDURE(), ' ', ERROR_LINE());
PRINT @vchError;

END CATCH
IF
  LEN(@vchError) > 0 BEGIN
      INSERT INTO @Result (bResult, vchMessage)
    VALUES
      (0,@vchError);
    
    END ELSE BEGIN
      INSERT INTO @Result (bResult, vchMessage, iPlanID)
    VALUES
      (1, '',@iPlanID);
    
  END;
SELECT
  * 
FROM
  @Result;

SET NOCOUNT OFF;

END;

GO
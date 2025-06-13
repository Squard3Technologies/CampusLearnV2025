CREATE OR ALTER PROCEDURE dbo.SP_UpdateModule
(
    @id UNIQUEIDENTIFIER,
    @code VARCHAR(50),
    @name NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(@id = CAST(0x00 AS UNIQUEIDENTIFIER))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 400
			SET @StatusMessage = 'Module Id is required to update a module'
		END		
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[Module] M WITH(NOLOCK) WHERE M.Id = @id))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'A module with the Id does not exists in the system.'
		END
		ELSE
		BEGIN	
			UPDATE T
			SET		T.Name = @name, T.Code = @code
			FROM dbo.Module T
			WHERE T.Id =@id 

			SET @Status = 1
			SET @StatusCode = 200
			SET @StatusMessage = 'Successful'
		END
	END TRY
	BEGIN CATCH
		SET @Status = 0
		SET @StatusCode = 500
		SET @StatusMessage = ERROR_MESSAGE()
	END CATCH
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END;
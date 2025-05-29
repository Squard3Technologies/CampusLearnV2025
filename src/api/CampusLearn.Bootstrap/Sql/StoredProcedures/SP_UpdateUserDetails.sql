CREATE OR ALTER PROCEDURE dbo.SP_UpdateUserDetails
(
	@id UNIQUEIDENTIFIER,
	@name VARCHAR(150),
	@surname VARCHAR(150),
	@contact VARCHAR(150),
	@email VARCHAR(150)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		UPDATE	U 
		SET		U.Name = @name,
				U.Surname = @surname,
				U.ContactNumber = @contact,
				U.Email = @email
		FROM dbo.[User] U WITH(NOLOCK)
		WHERE 
		(
			U.Id = @id
		)
		SET @Status = 1
		SET @StatusCode = 200
		SET @StatusMessage = 'Successful'
	END TRY
	BEGIN CATCH
		SET @Status = 0
		SET @StatusCode = 500
		SET @StatusMessage = ERROR_MESSAGE()
	END CATCH
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END
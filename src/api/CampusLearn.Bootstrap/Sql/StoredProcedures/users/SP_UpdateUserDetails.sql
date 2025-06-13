CREATE OR ALTER PROCEDURE dbo.SP_UpdateUserDetails
(
	@id UNIQUEIDENTIFIER,
	@name VARCHAR(150),
	@middlename VARCHAR(150) = NULL,
	@surname VARCHAR(150),
	@contact VARCHAR(150),
	@email VARCHAR(150)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(EXISTS(SELECT * FROM dbo.[User] U WITH(NOLOCK) WHERE U.Email = @email AND U.Id <> @id))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 409
			SET @StatusMessage = 'The provided email address is already linked to another account'
		END
		ELSE
		BEGIN
			UPDATE	U 
			SET		U.Name = @name,
					U.MiddleName = ISNULL(@middlename,''),
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
		END
	END TRY
	BEGIN CATCH
		SET @Status = 0
		SET @StatusCode = 500
		SET @StatusMessage = ERROR_MESSAGE()
	END CATCH
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END
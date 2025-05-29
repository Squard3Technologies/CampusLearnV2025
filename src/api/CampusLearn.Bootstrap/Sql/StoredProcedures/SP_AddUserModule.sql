CREATE OR ALTER PROCEDURE dbo.SP_AddUserModule
(
    @userId UNIQUEIDENTIFIER,
	@moduleId UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(NOT EXISTS(SELECT * FROM dbo.[Module] M WITH(NOLOCK) WHERE M.Id = @moduleId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'The selected module doesnot exists in the system.'
		END
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[User] M WITH(NOLOCK) WHERE M.Id = @userId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'The selected user does not exist in the system.'
		END
		ELSE IF(EXISTS(SELECT * FROM dbo.UserModule M WITH(NOLOCK) WHERE M.UserId = @userId AND M.ModuleId = @moduleId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 409
			SET @StatusMessage = 'The selected module is already linked to the user.'
		END
		ELSE
		BEGIN	
			INSERT INTO dbo.UserModule(UserId, ModuleId)
			VALUES(@userId, @moduleId)

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
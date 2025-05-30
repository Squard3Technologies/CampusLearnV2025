CREATE OR ALTER PROCEDURE dbo.SP_CreateTopic
(
    @id UNIQUEIDENTIFIER,
    @authorId UNIQUEIDENTIFIER,
	@moduleId NVARCHAR(255),
    @title NVARCHAR(255),
	@description NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(@id = CAST(0x00 AS UNIQUEIDENTIFIER))
		BEGIN
			SET @id = NEWID()
		END
		
		IF(EXISTS(SELECT * FROM dbo.[Topic] M WITH(NOLOCK) WHERE M.Id = @id))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 409
			SET @StatusMessage = 'A topic with the same Id already exists in the system. Please use update function if you intend to update the topic.'
		END
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[Module] M WITH(NOLOCK) WHERE M.Id = @moduleId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'A module with the specied id does not exists'
		END
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[User] U WITH(NOLOCK) WHERE U.Id = @authorId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'The specied user does not exists'
		END
		ELSE
		BEGIN	
			INSERT INTO dbo.[Topic](Id, Title, Description, CreatedByUserId, ModuleId)
			VALUES(@id, @title, @description, @authorId, @moduleId)

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
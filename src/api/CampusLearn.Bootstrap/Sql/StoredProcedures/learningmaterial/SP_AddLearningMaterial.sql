CREATE OR ALTER PROCEDURE dbo.SP_AddLearningMaterial
(
    @id UNIQUEIDENTIFIER,
    @uploadedByUserId UNIQUEIDENTIFIER,
    @topicId UNIQUEIDENTIFIER,
    @fileType VARCHAR(50),
    @filePath VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(@id = CAST(0x00 AS UNIQUEIDENTIFIER))
		BEGIN
			SET @id = NEWID()
		END
		
		IF(NOT EXISTS(SELECT * FROM dbo.[User] M WITH(NOLOCK) WHERE M.Id = @uploadedByUserId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'A user with specified Id does not exists in the system.'
		END
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[Topic] M WITH(NOLOCK) WHERE M.Id = @TopicId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'A Topic with the specified Id does not exist in the system.'
		END
		ELSE
		BEGIN	
			INSERT INTO dbo.LearningMaterial(Id, UploadedByUserId, TopicId, FileType, FilePath)
			VALUES(@id, @uploadedByUserId, @TopicId, @fileType, @filePath)

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
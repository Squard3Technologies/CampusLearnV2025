CREATE OR ALTER PROCEDURE dbo.SP_AddModule
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
			SET @id = NEWID()
		END
		
		IF(EXISTS(SELECT * FROM dbo.[Module] M WITH(NOLOCK) WHERE M.Id = @id))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 409
			SET @StatusMessage = 'A module with the same Id already exists in the system. Please use update function if you intend to update the module.'
		END
		ELSE IF(EXISTS(SELECT * FROM dbo.[Module] M WITH(NOLOCK) WHERE M.Code = @code))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 409
			SET @StatusMessage = 'A module with the same code already exists in the system. Please use update function if you intend to update the module.'
		END
		ELSE
		BEGIN	
			INSERT INTO dbo.Module(Id, Code,Name)
			VALUES(@id, @code, @name)

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
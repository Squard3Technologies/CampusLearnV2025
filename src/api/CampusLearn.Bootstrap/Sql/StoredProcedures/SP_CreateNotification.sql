CREATE OR ALTER PROCEDURE dbo.SP_CreateNotification
(
    @SenderId UNIQUEIDENTIFIER,
    @ReceiverId UNIQUEIDENTIFIER,
    @NotificationType INT,
    @Content NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(NOT EXISTS(SELECT * FROM dbo.[User] U WITH(NOLOCK) WHERE U.Id = @SenderId))
		BEGIN
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'Cannot sender account on the system'
		END
		ELSE IF(NOT EXISTS(SELECT * FROM dbo.[User] U WITH(NOLOCK) WHERE U.Id = @ReceiverId))
		BEGIN			
			SET @Status = 0
			SET @StatusCode = 404
			SET @StatusMessage = 'Cannot find recipient on the system'
		END
		ELSE
		BEGIN
			INSERT INTO dbo.[Notification] (
                Id,
                SenderId,
                ReceiverId,
                NotificationType,
                Content
            )
			VALUES (
                NEWID(),
                @SenderId,
                @ReceiverId, 
                @NotificationType,
                @Content
            );
			SET @Status = 1
			SET @StatusCode = 200
			SET @StatusMessage = 'Successfully queue message for processing'
		END		
	END TRY
	BEGIN CATCH
		SET @Status = 0
		SET @StatusCode = 500
		SET @StatusMessage = ERROR_MESSAGE()
	END CATCH
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END;
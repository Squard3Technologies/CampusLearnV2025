CREATE OR ALTER PROCEDURE dbo.SP_UpdateNotificationStatus
(
	@id UNIQUEIDENTIFIER,
    @statusMessage VARCHAR(255),
	@statusCode INT
)
AS
BEGIN
	UPDATE N
	SET		N.StatusCode = @statusCode, N.StatusMessage = @statusMessage	
	FROM dbo.[Notification] N WITH(NOLOCK) 
	WHERE 
	(
		N.Id = @id
	)
END
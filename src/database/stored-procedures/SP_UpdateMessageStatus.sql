CREATE OR ALTER PROCEDURE dbo.SP_UpdateMessageStatus
(
	@id UNIQUEIDENTIFIER,
    @statusMessage VARCHAR(255),
	@statusCode INT
)
AS
BEGIN
	UPDATE	M 
	SET		M.StatusCode = @statusCode, M.StatusMessage = @statusMessage	
	FROM dbo.[Message] M WITH(NOLOCK) 
	WHERE 
	(
		M.Id = @id
	)
END
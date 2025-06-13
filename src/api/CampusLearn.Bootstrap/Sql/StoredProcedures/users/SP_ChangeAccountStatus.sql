CREATE OR ALTER PROCEDURE dbo.SP_ChangeAccountStatus
(
	@userId UNIQUEIDENTIFIER,
	@accountStatusId UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		UPDATE	U 
		SET		U.AccountStatus = @accountStatusId
		FROM dbo.[User] U WITH(NOLOCK)
		WHERE 
		(
			U.Id = @userId
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
CREATE OR ALTER PROCEDURE dbo.SP_UpdateUserProfile
(
    @UserId UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @MiddleName NVARCHAR(255) = NULL,
    @Surname NVARCHAR(255),
    @ContactNumber NVARCHAR(20)
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)    
    UPDATE U
    SET
        U.Name = @Name,
        U.MiddleName = @MiddleName,
        U.Surname = @Surname,
        U.ContactNumber = @ContactNumber
    FROM [dbo].[User] U
    WHERE U.Id = @UserId

    SET @Status = 1
	SET @StatusCode = 200
	SET @StatusMessage = 'Successful'
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END
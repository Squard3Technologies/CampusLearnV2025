CREATE OR ALTER PROCEDURE dbo.SP_ChangePassword
(
    @UserId UNIQUEIDENTIFIER,
    @Password NVARCHAR(MAX)
)
AS
BEGIN
    UPDATE [CampusLearnDB].[dbo].[User] SET
        Password = @Password
    WHERE Id = @UserId
END;
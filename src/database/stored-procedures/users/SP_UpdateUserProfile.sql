CREATE OR ALTER PROCEDURE dbo.SP_UpdateUserProfile
(
    @UserId UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Surname NVARCHAR(255),
    @ContactNumber NVARCHAR(20)
)
AS
BEGIN
    UPDATE [CampusLearnDB].[dbo].[User] SET
        Name = @Name,
        Surname = @Surname,
        ContactNumber = @ContactNumber
    WHERE Id = @UserId
END;
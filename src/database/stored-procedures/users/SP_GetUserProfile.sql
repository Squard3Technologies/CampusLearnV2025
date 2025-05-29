CREATE OR ALTER PROCEDURE dbo.SP_GetUserProfile
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT u.Id, u.Name, u.Surname, u.Email, u.ContactNumber
    FROM [CampusLearnDB].[dbo].[User] u
    WHERE u.Id = @UserId

    SELECT m.Id, m.Code, m.Name
    FROM [CampusLearnDB].[dbo].[User] u
        INNER JOIN UserModule um ON u.Id = um.UserId
        INNER JOIN Module m ON m.Id = um.ModuleId
    WHERE u.Id = @UserId
END;
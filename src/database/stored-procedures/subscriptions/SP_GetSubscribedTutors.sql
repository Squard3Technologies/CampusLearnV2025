CREATE OR ALTER PROCEDURE dbo.SP_GetSubscribedTutors
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT ts.TutorId, u.Name, u.Surname, u.Email FROM TutorSubscription ts
        INNER JOIN [CampusLearnDB].[dbo].[User] u ON ts.TutorId = u.Id
    WHERE ts.UserId = @UserId

    SELECT um.UserId, um.ModuleId FROM [CampusLearnDB].[dbo].[User] u
        INNER JOIN UserModule um ON u.Id = um.ModuleId
    WHERE u.Id IN (SELECT TutorId FROM [TutorSubscription] ts WHERE ts.UserId = @UserId)

    SELECT * FROM Module
END;
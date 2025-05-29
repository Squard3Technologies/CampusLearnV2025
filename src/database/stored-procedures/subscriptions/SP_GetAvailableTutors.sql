CREATE OR ALTER PROCEDURE dbo.SP_GetAvailableTutors
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT u.Id 'TutorId', u.Name, u.Surname, u.Email FROM [CampusLearnDB].[dbo].[User] u
    WHERE u.Role = 3
        AND u.Id NOT IN (SELECT TutorId FROM [TutorSubscription] ts WHERE ts.UserId = @UserId)

    SELECT um.UserId, um.ModuleId FROM [CampusLearnDB].[dbo].[User] u
        INNER JOIN UserModule um ON u.Id = um.ModuleId
    WHERE u.Role = 3
        AND u.Id NOT IN (SELECT TutorId FROM [TutorSubscription] ts WHERE ts.UserId = @UserId)

    SELECT * FROM Module
END;
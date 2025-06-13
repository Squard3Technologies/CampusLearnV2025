CREATE OR ALTER PROCEDURE dbo.SP_GetSubscribedUserIdentifiers
(
    @UserId UNIQUEIDENTIFIER,
    @TopicId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT DISTINCT UserId
    FROM
    (
        SELECT ts.UserId FROM TopicSubscription ts
        WHERE ts.TopicId = @TopicId
        UNION
        SELECT ts.UserId FROM TutorSubscription ts
        WHERE ts.TutorId = @UserId
    ) AS CombinedUserIds
END;
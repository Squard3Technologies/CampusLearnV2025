CREATE OR ALTER PROCEDURE dbo.SP_GetAvailableTutors
(
    @TutorId UNIQUEIDENTIFIER,
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
        WHERE ts.TutorId = @TutorId
    ) AS CombinedUserIds
END;
CREATE OR ALTER PROCEDURE dbo.SP_GetSubscribedTopics
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT t.Id 'TopicId', t.Title 'TopicName', m.Id 'ModuleId', m.Code 'ModuleCode', m.Name 'ModuleName' FROM TopicSubscription ts
        INNER JOIN Topic t ON ts.TopicId = t.Id
        INNER JOIN Module m ON t.ModuleId = m.Id
    WHERE ts.UserId = @UserId
END;
CREATE OR ALTER PROCEDURE dbo.SP_GetQuizzesByTopic
(
    @TopicId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT q.Id, q.Title 'Name', q.Description, q.CreatedByUserId, t.Title 'TopicName', m.Code 'ModuleCode', q.Duration
    FROM Quiz q
        INNER JOIN Topic t ON q.TopicId = t.Id
        INNER JOIN Module m ON t.ModuleId = m.Id
    WHERE q.TopicId = @TopicId AND q.DateRemoved IS NULL
END;
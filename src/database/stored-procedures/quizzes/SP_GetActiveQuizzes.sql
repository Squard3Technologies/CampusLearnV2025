CREATE OR ALTER PROCEDURE dbo.SP_GetActiveQuizzes
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT q.Id, q.Title 'Name', q.Description, q.CreatedByUserId, t.Title 'TopicName', m.Code 'ModuleCode', q.Duration
    FROM QuizAttempt qa
        INNER JOIN Quiz q
        INNER JOIN Topic t ON q.TopicId = t.Id
        INNER JOIN Module m ON t.ModuleId = m.Id
    WHERE qa.UserId = @UserId AND DateAttempted IS NULL
END;
CREATE OR ALTER PROCEDURE dbo.SP_GetActiveQuizzes
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT q.Id, qa.Id 'QuizAttemptId', q.Title 'Name', q.Description, q.CreatedByUserId, t.Title 'TopicName', m.Code 'ModuleCode', q.Duration
    FROM QuizAttempt qa
        INNER JOIN Quiz q ON qa.QuizId = q.Id
        INNER JOIN Topic t ON q.TopicId = t.Id
        INNER JOIN Module m ON t.ModuleId = m.Id
    WHERE qa.UserId = @UserId AND DateAttempted IS NULL
END;
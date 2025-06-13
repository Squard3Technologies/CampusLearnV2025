CREATE OR ALTER PROCEDURE dbo.SP_GetQuizzesAttemptHistory
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT qa.Id, q.Id 'QuizId', q.Title 'QuizName', m.Code 'ModuleCode', t.Title 'TopicName', 
           q.Duration, qa.DateAttempted 'LastAttemptDateTime',
           CONCAT(CAST(SUM(CASE WHEN qo.IsCorrect = 1 THEN 1 ELSE 0 END) AS VARCHAR), ' / ',
                  CAST(COUNT(1) AS VARCHAR)) 'LastAttemptScore'
    FROM QuizAttempt qa
        INNER JOIN Quiz q ON qa.QuizId = q.Id
        INNER JOIN Topic t ON q.TopicId = t.Id
        INNER JOIN Module m ON t.ModuleId = m.Id
        INNER JOIN QuizAttemptQuestionAnswer qaa ON qa.Id = qaa.QuizAttemptId
        INNER JOIN QuestionOption qo ON qaa.QuestionOptionId = qo.Id
    WHERE qa.UserId = @UserId AND qa.DateAttempted IS NOT NULL
    GROUP BY qa.Id, q.Id, q.Title, m.Code, t.Title, q.Duration, qa.DateAttempted
    ORDER BY qa.DateAttempted DESC
END;
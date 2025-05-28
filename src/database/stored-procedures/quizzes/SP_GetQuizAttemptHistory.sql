CREATE OR ALTER PROCEDURE dbo.SP_GetQuizAttemptHistory
    @QuizAttemptId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT qa.Id, q.Title 'Name'
    FROM QuizAttempt qa
        INNER JOIN Quiz q ON qa.QuizId = q.Id
    WHERE qa.Id = @QuizAttemptId

    SELECT q.Id, q.Title 'Name' FROM QuizAttempt qa
        INNER JOIN QuizAttemptQuestionAnswer qaqa ON qaqa.QuizAttemptId = qa.Id
        INNER JOIN Question q ON q.Id = qaqa.QuestionId
    WHERE qa.Id = @QuizAttemptId
    GROUP BY q.Id, q.Title

    SELECT qo.Id, qo.QuestionId, qo.Value 'Name', qo.IsCorrect,
        CASE 
            WHEN qo.Id = qaqa.QuestionOptionId THEN 1 
            ELSE 0 
        END 'IsChosen'
    FROM QuizAttempt qa
        INNER JOIN QuizAttemptQuestionAnswer qaqa ON qaqa.QuizAttemptId = qa.Id
        INNER JOIN Question q ON q.Id = qaqa.QuestionId
        INNER JOIN QuestionOption qo ON qo.QuestionId = q.Id
    WHERE qa.Id = @QuizAttemptId
END
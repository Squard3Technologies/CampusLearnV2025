CREATE OR ALTER PROCEDURE dbo.SP_GetQuizDetail
    @QuizId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT q.Id, q.Title 'Name', q.Description
    FROM Quiz q
    WHERE q.Id = @QuizId

    SELECT q.Id, q.Title 'Name', q.QuestionType
    FROM Question q
    WHERE q.QuizId = @QuizId AND q.DateRemoved IS NULL

    SELECT qo.Id, qo.QuestionId, qo.Value 'Name', qo.IsCorrect
    FROM QuestionOption qo
        INNER JOIN Question q ON qo.QuestionId = q.Id
    WHERE q.QuizId = @QuizId AND q.DateRemoved IS NULL
END
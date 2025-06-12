CREATE OR ALTER PROCEDURE dbo.SP_CompleteQuizAttempt
(
    @QuizAttemptId UNIQUEIDENTIFIER,
	@AttemptDuration TIME,
    @AnswersJson NVARCHAR(MAX)
)
AS
BEGIN
    -- Create quiz attempt
    UPDATE QuizAttempt SET
		DateAttempted = GETDATE(),
		AttemptDuration = @AttemptDuration
	WHERE Id = @QuizAttemptId
		
    -- Parse JSON to table and insert answers
    DELETE QuizAttemptQuestionAnswer WHERE QuizAttemptId = @QuizAttemptId

    INSERT INTO QuizAttemptQuestionAnswer (Id, QuizAttemptId, QuestionId, QuestionOptionId, IsCorrect)
    SELECT 
        NEWID(),
        @QuizAttemptId,
        JSON_VALUE(js.value, '$.QuestionId'),
        JSON_VALUE(js.value, '$.SelectedQuestionOptionId'),
        qo.IsCorrect
    FROM OPENJSON(@AnswersJson) AS js
        JOIN QuestionOption qo ON qo.Id = JSON_VALUE(js.value, '$.SelectedQuestionOptionId');
END;
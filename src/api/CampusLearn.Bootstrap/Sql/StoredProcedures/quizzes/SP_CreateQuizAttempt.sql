CREATE OR ALTER PROCEDURE dbo.SP_CreateQuizAttempt
(
    @QuizId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @AssignedByUserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Status BIT, @StatusCode INT, @StatusMessage NVARCHAR(MAX);
    DECLARE @QuizAttemptId UNIQUEIDENTIFIER;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Quiz WHERE Id = @QuizId)
        BEGIN
            SET @Status = 0;
            SET @StatusCode = 404;
            SET @StatusMessage = 'Quiz not found';
            SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage];
            RETURN;
        END

        SET @QuizAttemptId = NEWID();

        -- Create quiz attempt
        INSERT INTO QuizAttempt (Id, QuizId, UserId, DateCreated, AssignedByUserId)
        VALUES (@QuizAttemptId, @QuizId, @UserId, GETDATE(), @AssignedByUserId);

        SET @Status = 1;
        SET @StatusCode = 200;
        SET @StatusMessage = 'Quiz attempt and answers successfully created';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @StatusCode = 500;
        SET @StatusMessage = ERROR_MESSAGE();
    END CATCH

    SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage], @QuizAttemptId AS [Body];
END;
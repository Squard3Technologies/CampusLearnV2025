CREATE OR ALTER PROCEDURE dbo.SP_UpsertQuizQuestion
(
    @QuestionId UNIQUEIDENTIFIER NULL,
    @QuizId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @QuestionType INT,
    @QuestionOptionsJson NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Status BIT, @StatusCode INT, @StatusMessage NVARCHAR(MAX);

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Quiz WHERE Id = @QuizId)
        BEGIN
            SET @Status = 0;
            SET @StatusCode = 404;
            SET @StatusMessage = 'Quiz not found';
            SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage];
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM User WHERE Id = @UserId)
        BEGIN
            SET @Status = 0;
            SET @StatusCode = 404;
            SET @StatusMessage = 'User not found';
            SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage];
            RETURN;
        END

        IF(ISNULL(@QuestionId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER))
        BEGIN
            SET @QuestionId = NEWID()
        END

        IF(NOT EXISTS(SELECT * FROM dbo.Question Q WITH(NOLOCK) WHERE Q.Id = @QuestionId))
        BEGIN
            --Create question
            INSERT INTO Question (Id, QuizId, Title, QuestionType, DateCreated)
            VALUES (@QuestionId, @QuizId, @UserId, @QuestionType, GETDATE());

            --Create question options
            INSERT INTO QuestionOption (Id, QuestionId, [Value], IsCorrect, DateCreated)
            SELECT 
                NEWID(),
                @QuestionId,
                JSON_VALUE(js.value, '$.Value'),
                JSON_VALUE(js.value, '$.IsCorrect'),
                GETDATE()
            FROM OPENJSON(@QuestionOptionsJson) AS js
        END
        ELSE
        BEGIN
            -- Update existing question
            UPDATE Question
            SET 
                Title = @Name,
                QuestionType = @QuestionType
            WHERE Id = @QuestionId;

            -- Upsert logic for QuestionOptions using MERGE
            MERGE INTO QuestionOption AS target
            USING (
                SELECT
                    JSON_VALUE(js.value, '$.Id') AS Id,
                    @QuestionId AS QuestionId,
                    JSON_VALUE(js.value, '$.Value') AS [Value],
                    JSON_VALUE(js.value, '$.IsCorrect') AS IsCorrect
                FROM OPENJSON(@QuestionOptionsJson) AS js
            ) AS source
            ON target.Id = source.Id

            WHEN MATCHED THEN 
                UPDATE SET
                    target.[Value] = source.[Value],
                    target.IsCorrect = source.IsCorrect

            WHEN NOT MATCHED BY TARGET THEN
                INSERT (Id, QuestionId, [Value], IsCorrect, DateCreated)
                VALUES (
                    ISNULL(source.Id, NEWID()), 
                    source.QuestionId, 
                    source.[Value], 
                    source.IsCorrect, 
                    GETDATE()
                );

            --Delete old options
            UPDATE QuestionOption SET
                DateRemoved = GETDATE(),
                RemovedByUserId = @UserId
            WHERE QuestionId = @QuestionId
                AND Id NOT IN (
                    SELECT JSON_VALUE(js.value, '$.Id')
                    FROM OPENJSON(@QuestionOptionsJson) AS js
                    WHERE ISJSON(js.value) = 1
                );
        END

        SET @Status = 1;
        SET @StatusCode = 200;
        SET @StatusMessage = 'Quiz question and options successfully processed';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @StatusCode = 500;
        SET @StatusMessage = ERROR_MESSAGE();
    END CATCH

    SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage], @QuizAttemptId AS [Body];
END;
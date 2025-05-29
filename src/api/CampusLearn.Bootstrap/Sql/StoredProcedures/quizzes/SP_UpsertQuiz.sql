CREATE OR ALTER PROCEDURE dbo.SP_UpsertQuiz
(
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @TopicId UNIQUEIDENTIFIER,
    @Duration TIME,
    @CreatedByUserId UNIQUEIDENTIFIER,
    @QuizId UNIQUEIDENTIFIER NULL
)
AS
BEGIN
    DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX);

    BEGIN TRY
        IF (NOT EXISTS (SELECT 1 FROM dbo.[Topic] T WITH(NOLOCK) WHERE T.Id = @TopicId))
        BEGIN
            SET @Status = 0
            SET @StatusCode = 404
            SET @StatusMessage = 'Cannot find topic for TopicId'
        END
        ELSE
        BEGIN
            IF(ISNULL(@QuizId, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER))
            BEGIN
                SET @QuizId = NEWID()
            END

            IF(NOT EXISTS(SELECT * FROM dbo.Quiz Q WITH(NOLOCK) WHERE Q.Id = @QuizId))
            BEGIN
                INSERT INTO dbo.Quiz (
                    Id,
                    Title,
                    Description,
                    CreatedByUserId,
                    TopicId,
                    DateCreated,
                    Duration
                )
                VALUES (
                    @QuizId,
                    @Name,
                    @Description,
                    @CreatedByUserId,
                    @TopicId,
                    GETDATE(),
                    @Duration
                );
            END
            ELSE
            BEGIN
                UPDATE dbo.Quiz SET
                    Title = @Name,
                    Description = @Description,
                    Duration = @Duration
                WHERE Id = @QuizId
            END

            SET @Status = 1
            SET @StatusCode = 200
            SET @StatusMessage = 'Quiz successfully processed'
        END
    END TRY
    BEGIN CATCH
        SET @Status = 0
        SET @StatusCode = 500
        SET @StatusMessage = ERROR_MESSAGE()
    END CATCH

    SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage], @QuizId AS [Body]
END;
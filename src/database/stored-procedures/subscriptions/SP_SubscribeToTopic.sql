CREATE OR ALTER PROCEDURE dbo.SP_SubscribeToTopic
(
    @UserId UNIQUEIDENTIFIER,
    @TopicId UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER;
    IF(NOT EXISTS(SELECT * FROM [CampusLearnDB].[dbo].[TopicSubscription] TS WITH(NOLOCK)
        WHERE TS.UserId = @UserId AND TS.TopicId = @TopicId))
    BEGIN
        SET @Id = NEWID();

        INSERT INTO [CampusLearnDB].[dbo].[TopicSubscription] (Id, UserId, TopicId, DateSubscribed)
        VALUES (@Id, @UserId, @TopicId, GETDATE());
    END

    SELECT @Id;
END;
CREATE OR ALTER PROCEDURE dbo.SP_UnsubscribeFromTopic
(
    @UserId UNIQUEIDENTIFIER,
    @TopicId UNIQUEIDENTIFIER
)
AS
BEGIN
    DELETE [CampusLearnDB].[dbo].[TopicSubscription]
    WHERE UserId = @UserId AND TopicId = @TopicId
END;
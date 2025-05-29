CREATE OR ALTER PROCEDURE dbo.SP_UnsubscribeFromTutor
(
    @UserId UNIQUEIDENTIFIER,
    @TutorId UNIQUEIDENTIFIER
)
AS
BEGIN
    DELETE [CampusLearnDB].[dbo].[TutorSubscription]
    WHERE UserId = @UserId AND TutorId = @TutorId
END;
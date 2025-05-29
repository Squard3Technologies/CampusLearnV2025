CREATE OR ALTER PROCEDURE dbo.SP_SubscribeToTutor
(
    @UserId UNIQUEIDENTIFIER,
    @TutorId UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER;
    IF(NOT EXISTS(SELECT * FROM [CampusLearnDB].[dbo].[TutorSubscription] TS WITH(NOLOCK)
        WHERE TS.UserId = @UserId AND TS.TutorId = @TutorId))
    BEGIN
        SET @Id = NEWID();

        INSERT INTO [CampusLearnDB].[dbo].[TutorSubscription] (Id, UserId, TutorId, DateSubscribed)
        VALUES (@Id, @UserId, @TutorId, GETDATE());
    END

    SELECT @Id;
END;
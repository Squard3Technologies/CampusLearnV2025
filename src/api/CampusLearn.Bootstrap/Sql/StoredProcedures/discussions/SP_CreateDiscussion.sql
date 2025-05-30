CREATE OR ALTER PROCEDURE dbo.SP_CreateDiscussion
(
    @TopicId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Title NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [CampusLearnDB].[dbo].[Discussion] (Id, Title, CreatedByUserId, TopicId, DateCreated)
    VALUES (@Id, @Title, @UserId, @TopicId, GETDATE());

    SELECT @Id;
END;
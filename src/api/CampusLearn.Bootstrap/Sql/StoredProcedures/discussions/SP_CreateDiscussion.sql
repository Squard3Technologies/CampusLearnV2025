CREATE OR ALTER PROCEDURE dbo.SP_CreateDiscussion
(
    @TopicId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Title NVARCHAR(MAX),
    @Content NVARCHAR(MAX) = NULL
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [CampusLearnDB].[dbo].[Discussion] (Id, Title, Content, CreatedByUserId, TopicId, DateCreated)
    VALUES (@Id, @Title, @Content, @UserId, @TopicId, GETDATE());

    SELECT @Id;
END;
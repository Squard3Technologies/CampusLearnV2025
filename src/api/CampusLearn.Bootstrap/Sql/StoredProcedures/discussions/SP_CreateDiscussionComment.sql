CREATE OR ALTER PROCEDURE dbo.SP_CreateDiscussionComment
(
    @DiscussionId UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Content NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [CampusLearnDB].[dbo].[Comment] (Id, DiscussionId, CreatedByUserId, Content, CreatedDate)
    VALUES (@Id, @DiscussionId, @UserId, @Content, GETDATE());

    SELECT @Id;
END;
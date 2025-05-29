CREATE OR ALTER PROCEDURE dbo.SP_CreateChatMessage
(
    @SenderId UNIQUEIDENTIFIER,
    @ReceiverId UNIQUEIDENTIFIER,
    @Content NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [CampusLearnDB].[dbo].[Message] (Id, SenderId, ReceiverId, Content, DateCreated)
    VALUES (@Id, @SenderId, @ReceiverId, @Content, GETDATE());

    SELECT @Id;
END;
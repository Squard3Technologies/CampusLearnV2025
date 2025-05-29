CREATE OR ALTER PROCEDURE dbo.SP_GetChatMessages
(
    @SenderId UNIQUEIDENTIFIER,
    @ReceiverId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT m.Id, m.SenderId 'userId', sender.Name 'UserName', sender.Surname 'UserSurname', sender.Email 'UserEmail', m.Content, m.DateCreated
    FROM [CampusLearnDB].[dbo].[Message] m
        INNER JOIN [CampusLearnDB].[dbo].[User] sender ON sender.Id = m.SenderId
        INNER JOIN [CampusLearnDB].[dbo].[User] receiver ON receiver.Id = m.ReceiverId
    WHERE (m.SenderId = @SenderId AND m.ReceiverId = @ReceiverId)
        OR (m.ReceiverId = @SenderId AND m.SenderId = @ReceiverId)
    ORDER BY m.DateCreated
END;
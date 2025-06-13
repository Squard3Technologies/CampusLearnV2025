CREATE OR ALTER PROCEDURE dbo.SP_GetChatsByUser
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    -- Get users the current user has sent messages to
    SELECT 
        otherUser.Id 'UserId', 
        otherUser.Name 'UserName', 
        otherUser.Surname 'UserSurname', 
        otherUser.Email 'UserEmail',
        (SELECT TOP 1 Content FROM [CampusLearnDB].[dbo].[Message] 
         WHERE (SenderId = @UserId AND ReceiverId = otherUser.Id) 
            OR (SenderId = otherUser.Id AND ReceiverId = @UserId)
         ORDER BY DateCreated DESC) as 'LastMessage',
        (SELECT TOP 1 DateCreated FROM [CampusLearnDB].[dbo].[Message] 
         WHERE (SenderId = @UserId AND ReceiverId = otherUser.Id) 
            OR (SenderId = otherUser.Id AND ReceiverId = @UserId)
         ORDER BY DateCreated DESC) as 'LastMessageTime'
    FROM 
        [CampusLearnDB].[dbo].[User] otherUser
    WHERE 
        EXISTS (
            SELECT 1 FROM [CampusLearnDB].[dbo].[Message] m 
            WHERE ((m.SenderId = @UserId AND m.ReceiverId = otherUser.Id) 
               OR (m.SenderId = otherUser.Id AND m.ReceiverId = @UserId))
               AND m.MessageType = 'Chat'
        )
        AND otherUser.Id <> @UserId
    ORDER BY LastMessageTime DESC
END;
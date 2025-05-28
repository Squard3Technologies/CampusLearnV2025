CREATE OR ALTER PROCEDURE dbo.SP_GetChatsByUser
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT m.ReceiverId 'UserId', u.Name 'UserName', u.Surname 'UserSurname', u.Email 'UserEmail' FROM [CampusLearnDB].[dbo].[Message] m
        INNER JOIN [CampusLearnDB].[dbo].[User] u ON m.ReceiverId = u.Id
    WHERE m.SenderId = @UserId
    GROUP BY m.ReceiverId, u.Name, u.Surname, u.Email
END;
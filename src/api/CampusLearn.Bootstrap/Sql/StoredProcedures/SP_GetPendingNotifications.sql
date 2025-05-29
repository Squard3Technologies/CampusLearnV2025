CREATE OR ALTER PROCEDURE dbo.SP_GetPendingNotifications
(
    @notificationType INT,
	@batchSize INT
)
AS
BEGIN
	SELECT	TOP (@batchSize)
			N.Id AS ID, 
			S.Id AS SenderID,
			S.Name AS SenderName, 
			S.Surname AS SenderSurname,
			S.Email AS SenderEmailAddress,
			R.Id AS ReceiverID,
			R.Name AS ReceiverName, 
			R.Surname AS ReceiverSurname,
			R.Email AS ReceiverEmailAddress,
			N.Content AS MessageBody,
			N.NotificationType AS NotificationType
	FROM dbo.[Notification] N WITH(NOLOCK) 
	INNER JOIN dbo.[User] S WITH(NOLOCK) ON S.Id = N.SenderId
	INNER JOIN dbo.[User] R WITH(NOLOCK) ON R.Id = N.ReceiverId
	WHERE 
	(
		N.NotificationType = @notificationType
		AND 
		N.StatusCode = -1
		AND
		ISNULL(N.StatusMessage,'') = ''
	)
END
CREATE OR ALTER PROCEDURE dbo.SP_GetPendingMessages
(
    @messageType VARCHAR(255),
	@batchSize INT
)
AS
BEGIN
	SELECT	TOP (@batchSize)
			M.Id AS ID, 
			S.Id AS SenderID,
			S.Name AS SenderName, 
			S.Surname AS SenderSurname,
			S.Email AS SenderEmailAddress,
			R.Id AS ReceiverID,
			R.Name AS ReceiverName, 
			R.Surname AS ReceiverSurname,
			R.Email AS ReceiverEmailAddress,
			M.Content AS MessageBody
			--M.MessageType AS MessageType
	FROM dbo.[Message] M WITH(NOLOCK) 
	INNER JOIN dbo.[User] S WITH(NOLOCK) ON S.Id = M.SenderId
	INNER JOIN dbo.[User] R WITH(NOLOCK) ON R.Id = M.ReceiverId
	WHERE 
	(
		--M.MessageType = @messageType
		--AND 
		M.StatusCode = -1
		AND
		ISNULL(M.StatusMessage,'') = ''
	)
END
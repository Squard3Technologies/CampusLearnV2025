CREATE OR ALTER PROCEDURE dbo.SP_EnquiriesByStatus
(
    @Status INT
)
AS
BEGIN
    SELECT e.*, t.Title 'TopicTitle', cu.Email 'CreatedByUserEmail', ru.Email 'ResolvedByUserEmail' FROM Enquiry e
		INNER JOIN dbo.[User] cu ON cu.Id = e.CreatedByUserId
		LEFT JOIN dbo.[User] ru ON ru.Id = e.ResolvedByUserId
	    LEFT JOIN Topic t ON e.LinkedTopicId = t.Id
    WHERE e.Status = @Status;
END;
CREATE OR ALTER PROCEDURE dbo.SP_EnquiriesByStatus
(
    @Status INT
)
AS
BEGIN
    SELECT e.*, t.Title 'TopicTitle' FROM Enquiry e
	    LEFT JOIN Topic t ON e.LinkedTopicId = t.Id
    WHERE e.Status = @Status;
END;
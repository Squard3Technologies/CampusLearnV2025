CREATE OR ALTER PROCEDURE dbo.SP_EnquiriesByUser
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT e.*, t.Title 'TopicTitle' FROM Enquiry e
	    LEFT JOIN Topic t ON e.LinkedTopicId = t.Id
    WHERE e.CreatedByUserId = @UserId;
END;

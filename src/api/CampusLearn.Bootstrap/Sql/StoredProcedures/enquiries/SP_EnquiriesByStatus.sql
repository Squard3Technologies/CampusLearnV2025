CREATE OR ALTER PROCEDURE dbo.SP_EnquiriesByStatus
(
    @Status INT
)
AS
BEGIN
    SELECT * FROM Enquiry WHERE Status = @Status;
END;
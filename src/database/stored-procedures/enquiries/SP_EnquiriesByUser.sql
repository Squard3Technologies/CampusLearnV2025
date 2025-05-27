CREATE OR ALTER PROCEDURE dbo.SP_EnquiriesByUser
(
    @UserId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT * FROM Enquiry WHERE CreatedByUserId = @UserId;
END;

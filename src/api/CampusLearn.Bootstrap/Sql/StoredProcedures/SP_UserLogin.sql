CREATE OR ALTER PROCEDURE dbo.SP_UserLogin
(
    @EmailAddress VARCHAR(255)
)
AS
BEGIN
	SELECT	U.Id,
			U.Name AS [FirstName],
			U.Surname,
			U.ContactNumber,
			U.Email AS [EmailAddress],
			U.Password,
			U.Role,
			S.Id as AccountStatusId,
			S.Description AS AccountStatusDescription
	FROM dbo.[User] U WITH(NOLOCK) 
	INNER JOIN [dbo].[Statuses] S WITH(NOLOCK) ON S.Id = U.AccountStatus
	WHERE U.Email = @EmailAddress AND U.AccountStatus <> '114F6BEE-6EF5-47CA-B2B5-113106C1E5B2'
END;
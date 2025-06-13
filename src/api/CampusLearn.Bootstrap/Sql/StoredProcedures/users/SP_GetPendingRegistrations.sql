CREATE OR ALTER PROCEDURE dbo.SP_GetPendingRegistrations
AS
BEGIN
	SELECT	U.Id,
			U.Name AS [FirstName],
			U.MiddleName,
			U.Surname,
			U.ContactNumber,
			U.Email AS [EmailAddress],
			U.Password,
			U.Role,
			S.Id as AccountStatusId,
			S.Description AS AccountStatusDescription
	FROM dbo.[User] U WITH(NOLOCK)
	INNER JOIN [dbo].[Statuses] S WITH(NOLOCK) ON S.Id = U.AccountStatus
	WHERE 
	(
		U.AccountStatus = '7DCF4027-85AA-4C08-92FF-F3A669DFF157'
	)
	ORDER BY U.Name, U.Surname
END
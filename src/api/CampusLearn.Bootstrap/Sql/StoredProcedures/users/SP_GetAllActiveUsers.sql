CREATE OR ALTER PROCEDURE dbo.SP_GetAllActiveUsers
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
	WHERE 
	(
		U.AccountStatus IN
		(
			'B492BB30-B073-4059-A58B-B3F6E5BE4C95'
		)
	)
	ORDER BY U.Name, U.Surname
END
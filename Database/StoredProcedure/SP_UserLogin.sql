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
			U.Role
	FROM dbo.[User] U WITH(NOLOCK) 
	WHERE U.Email = @EmailAddress
END;
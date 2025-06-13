CREATE OR ALTER PROCEDURE dbo.SP_GetUserModules
(
    @userId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT m.* 
	FROM dbo.Module m WITH(NOLOCK)
		INNER JOIN dbo.UserModule us ON us.ModuleId = m.Id
	WHERE us.UserId = @userId
	ORDER BY m.Name
END;
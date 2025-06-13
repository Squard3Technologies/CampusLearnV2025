CREATE OR ALTER PROCEDURE dbo.SP_GetUserModuleLinks
(
    @userId UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT	UM.ModuleId, M.Code AS [ModuleCode], M.Name AS [ModuleName],
			U.*
	FROM dbo.UserModule UM WITH(NOLOCK)
	INNER JOIN dbo.Module M WITH(NOLOCK) ON M.Id = UM.ModuleId
	INNER JOIN dbo.[User] U WITH(NOLOCK) ON U.Id = UM.UserId
	WHERE UM.UserId = @userId
	ORDER BY M.Name
END
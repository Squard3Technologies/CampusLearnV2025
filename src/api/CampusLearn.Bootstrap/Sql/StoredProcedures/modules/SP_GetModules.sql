CREATE OR ALTER PROCEDURE dbo.SP_GetModules
AS
BEGIN
	SELECT * 
	FROM dbo.Module M WITH(NOLOCK)
	ORDER BY M.code, M.Name
END;
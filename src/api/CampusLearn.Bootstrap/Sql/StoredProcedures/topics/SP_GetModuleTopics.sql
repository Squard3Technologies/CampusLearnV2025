CREATE OR ALTER PROCEDURE dbo.SP_GetModuleTopics
(
	@moduleId NVARCHAR(255)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM dbo.[Topic] T WHERE T.ModuleId = @moduleId
END
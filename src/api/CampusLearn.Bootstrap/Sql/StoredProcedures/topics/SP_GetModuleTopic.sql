CREATE OR ALTER PROCEDURE dbo.SP_GetModuleTopic
(
	@moduleId UNIQUEIDENTIFIER,
	@topicId UNIQUEIDENTIFIER
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM dbo.[Topic] t WHERE t.ModuleId = @moduleId AND t.Id = @topicId
END
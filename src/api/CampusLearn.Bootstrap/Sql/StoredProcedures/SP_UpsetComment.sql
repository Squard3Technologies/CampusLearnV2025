
CREATE OR ALTER PROC SP_UpsetComment
(
	@Id UNIQUEIDENTIFIER = NULL,
	@Content NVARCHAR(MAX),
	@CreatedByUserId UNIQUEIDENTIFIER,
	@DiscussionId UNIQUEIDENTIFIER = NULL,
	@EnquiryId UNIQUEIDENTIFIER = NULL
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	BEGIN TRY
		IF(ISNULL(@Id, CAST(0x0 AS UNIQUEIDENTIFIER)) = CAST(0x0 AS UNIQUEIDENTIFIER))
		BEGIN
			SET @Id = NEWID()
		END

		IF(NOT EXISTS(SELECT * FROM dbo.Comment C WITH(NOLOCK) WHERE C.Id = @Id))
		BEGIN
			SET @Id = NEWID()
			INSERT INTO dbo.Comment(Id, Content, CreatedByUserId, DiscussionId, EnquiryId)
			VALUES(@Id, @Content, @CreatedByUserId, @DiscussionId, @EnquiryId)
			SET @Status = 1
			SET @StatusCode = 200
			SET @StatusMessage = 'SUCCESSFUL'
		END
		ELSE
		BEGIN
			UPDATE	T 
			SET		T.Content = @Content,
					T.CreatedByUserId = @CreatedByUserId,
					T.DiscussionId = @DiscussionId,
					T.EnquiryId = @EnquiryId
			FROM dbo.Comment T WITH(NOLOCK)
			WHERE T.Id = @Id

			SET @Status = 1
			SET @StatusCode = 201
			SET @StatusMessage = 'SUCCESSFUL'
		END
	END TRY
	BEGIN CATCH
		SET @Status = 0
		SET @StatusCode = 500
		SET @StatusMessage = '<p>Error Line: '+ CAST(ERROR_LINE() AS VARCHAR(10))+' <br /> Error Message: ' + ERROR_MESSAGE() +'</p>'
	END CATCH
END
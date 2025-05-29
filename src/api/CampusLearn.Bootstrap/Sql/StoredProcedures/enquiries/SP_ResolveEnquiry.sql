CREATE OR ALTER PROCEDURE dbo.SP_ResolveEnquiry
(
    @EnquiryId UNIQUEIDENTIFIER,
    @ResolvedByUserId NVARCHAR(255),
    @Status INT,
    @ResolutionAction INT,
    @ResolutionResponse NVARCHAR(MAX),
    @LinkedTopicId UNIQUEIDENTIFIER NULL
)
AS
BEGIN
    DECLARE @ResponseStatus BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)

    BEGIN TRY
        IF (NOT EXISTS (SELECT 1 FROM dbo.[Enquiry] E WITH(NOLOCK) WHERE E.Id = @EnquiryId AND E.Status = 1))
        BEGIN
            SET @ResponseStatus = 0
            SET @StatusCode = 404
            SET @StatusMessage = 'Cannot find pending enquiry for EnquiryId'
        END
        ELSE IF (NOT EXISTS (SELECT 1 FROM dbo.[User] U WITH(NOLOCK) WHERE U.Id = @ResolvedByUserId))
        BEGIN
            SET @ResponseStatus = 0
            SET @StatusCode = 404
            SET @StatusMessage = 'Cannot find user account for ResolvedByUserId'
        END
        ELSE IF (@LinkedTopicId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.[Topic] T WITH(NOLOCK) WHERE T.Id = @LinkedTopicId))
        BEGIN
            SET @ResponseStatus = 0
            SET @ResponseStatus = 404
            SET @StatusMessage = 'Cannot find linked topic'
        END
        BEGIN
            UPDATE dbo.Enquiry SET
                DateResolved = GETDATE(),
                ResolvedByUserId = @ResolvedByUserId,
                ResolutionAction = @ResolutionAction,
                ResolutionResponse = @ResolutionResponse,
                LinkedTopicId = @LinkedTopicId,
                Status = @Status
            WHERE Id = @EnquiryId;

            SET @ResponseStatus = 1
            SET @StatusCode = 200
            SET @StatusMessage = 'Enquiry successfully resolved'
        END
    END TRY
    BEGIN CATCH
        SET @ResponseStatus = 0
        SET @StatusCode = 500
        SET @StatusMessage = ERROR_MESSAGE()
    END CATCH

    SELECT @ResponseStatus AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END;
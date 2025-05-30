CREATE OR ALTER PROCEDURE dbo.SP_CreateEnquiry
(
    @CreatedByUserId UNIQUEIDENTIFIER,
    @Title NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @EnquiryStatus INT,
    @ModuleId UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)

    BEGIN TRY
        IF (NOT EXISTS (SELECT 1 FROM dbo.[User] U WITH(NOLOCK) WHERE U.Id = @CreatedByUserId))
        BEGIN
            SET @Status = 0
            SET @StatusCode = 404
            SET @StatusMessage = 'Cannot find user account for CreatedByUserId'
        END
        ELSE IF (@ModuleId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Module M WITH(NOLOCK) WHERE M.Id = @ModuleId))
        BEGIN
            SET @Status = 0
            SET @StatusCode = 404
            SET @StatusMessage = 'Cannot find module'
        END
        ELSE
        BEGIN
            INSERT INTO dbo.Enquiry (
                Id,
                CreatedByUserId,
                Title,
                Description,
                Status,
                ModuleId,
                DateCreated
            )
            VALUES (
                NEWID(),
                @CreatedByUserId,
                @Title,
                @Description,
                @EnquiryStatus,
                @ModuleId,
                GETDATE()
            );

            SET @Status = 1
            SET @StatusCode = 200
            SET @StatusMessage = 'Enquiry successfully created'
        END
    END TRY
    BEGIN CATCH
        SET @Status = 0
        SET @StatusCode = 500
        SET @StatusMessage = ERROR_MESSAGE()
    END CATCH

    SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END

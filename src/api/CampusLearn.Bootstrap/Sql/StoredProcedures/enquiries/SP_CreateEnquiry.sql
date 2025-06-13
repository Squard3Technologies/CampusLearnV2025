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
    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

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
        @Id,
        @CreatedByUserId,
        @Title,
        @Description,
        @EnquiryStatus,
        @ModuleId,
        GETDATE()
    );

    SELECT @Id;
END

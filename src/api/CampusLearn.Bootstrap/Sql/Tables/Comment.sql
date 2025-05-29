IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Comment')
BEGIN
    CREATE TABLE Comment (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Content NVARCHAR(MAX) NOT NULL,
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        CreatedDate DATETIME DEFAULT GETDATE(),
        DiscussionId UNIQUEIDENTIFIER NULL,
        EnquiryId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (DiscussionId) REFERENCES Discussion(Id),
        FOREIGN KEY (EnquiryId) REFERENCES Enquiry(Id),
        CONSTRAINT CHK_Comment_DiscussionOrEnquiry CHECK (
            (DiscussionId IS NOT NULL AND EnquiryId IS NULL) OR
            (DiscussionId IS NULL AND EnquiryId IS NOT NULL)
        ),
        INDEX IX_CreatedDate (CreatedDate ASC) INCLUDE(DiscussionId, CreatedByUserId)
    )
END
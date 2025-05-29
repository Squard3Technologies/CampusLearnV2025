

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enquiry')
BEGIN
    CREATE TABLE Enquiry (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        DateResolved DATETIME NULL,
        ResolvedByUserId UNIQUEIDENTIFIER NULL,
        Status INT NOT NULL,
        TopicId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (ResolvedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(ResolvedByUserId, TopicId)
    )
END
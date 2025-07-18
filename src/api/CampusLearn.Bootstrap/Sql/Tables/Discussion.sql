IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Discussion')
BEGIN
    CREATE TABLE Discussion (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        TopicId UNIQUEIDENTIFIER NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(CreatedByUserId, TopicId)
    )
END
ELSE
BEGIN
    -- Add Content column if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Discussion' AND COLUMN_NAME = 'Content')
    BEGIN
        ALTER TABLE Discussion ADD Content NVARCHAR(MAX) NULL;
    END
END
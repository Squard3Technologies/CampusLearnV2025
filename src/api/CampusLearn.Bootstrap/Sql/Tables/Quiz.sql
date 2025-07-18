IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Quiz')
BEGIN
    CREATE TABLE Quiz (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(255) NOT NULL,
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        TopicId UNIQUEIDENTIFIER NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        Duration TIME NULL,
        RemovedByUserId UNIQUEIDENTIFIER NULL,
        DateRemoved DATETIME NULL,
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (RemovedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(CreatedByUserId, TopicId)
    );
END;
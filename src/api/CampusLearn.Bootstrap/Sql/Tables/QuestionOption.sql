IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuestionOption')
BEGIN
    CREATE TABLE QuestionOption (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        QuestionId UNIQUEIDENTIFIER NOT NULL,
        Value NVARCHAR(MAX) NOT NULL,
        IsCorrect BIT DEFAULT 0,
        DateCreated DATETIME DEFAULT GETDATE(),
        DateRemoved DATETIME NULL,
        RemovedByUserId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (QuestionId) REFERENCES Question(Id),
        FOREIGN KEY (RemovedByUserId) REFERENCES [User](Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(QuestionId, IsCorrect)
    );
END;
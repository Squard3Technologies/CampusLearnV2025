IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LearningMaterial')
BEGIN
    CREATE TABLE LearningMaterial (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UploadedByUserId UNIQUEIDENTIFIER NOT NULL,
        TopicId UNIQUEIDENTIFIER NOT NULL,
        FileType NVARCHAR(50) NOT NULL,
        FilePath NVARCHAR(MAX) NOT NULL,
        UploadedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (UploadedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_UploadedDate (UploadedDate ASC) INCLUDE(FileType, UploadedByUserId)
    )
END
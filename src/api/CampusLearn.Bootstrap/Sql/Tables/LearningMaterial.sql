IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LearningMaterial')
BEGIN
    CREATE TABLE LearningMaterial (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        FileType NVARCHAR(50) NOT NULL,
        FilePath NVARCHAR(MAX) NOT NULL,
        UploadedByUserId UNIQUEIDENTIFIER NOT NULL,
        UploadedDate DATETIME DEFAULT GETDATE(),
        TopicId UNIQUEIDENTIFIER NOT NULL,
        FOREIGN KEY (UploadedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_UploadedDate (UploadedDate ASC) INCLUDE(FileType, UploadedByUserId)
    )
END
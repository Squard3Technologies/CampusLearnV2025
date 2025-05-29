IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enquiry')
BEGIN
    CREATE TABLE Enquiry (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        DateResolved DATETIME NULL,
        ResolvedByUserId UNIQUEIDENTIFIER NULL,
        ResolutionAction INT NULL,
        ResolutionResponse NVARCHAR(MAX) NULL,
        Status INT NOT NULL,
        ModuleId UNIQUEIDENTIFIER NOT NULL,
        LinkedTopicId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (ResolvedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (LinkedTopicId) REFERENCES Topic(Id),
        FOREIGN KEY (ModuleId) REFERENCES Module(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(ResolvedByUserId, LinkedTopicId)
    );
END;
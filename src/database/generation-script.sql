IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CampusLearnDB')
BEGIN
    CREATE DATABASE CampusLearnDB;
END;
GO

--IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'CLDBAdmin')
--BEGIN
--    CREATE LOGIN CLDBAdmin WITH PASSWORD = 'SEN371';
--END;
--GO

USE CampusLearnDB;
GO

--IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'CLDBAdmin')
--BEGIN
--    CREATE USER [CLDBAdmin] FOR LOGIN [CLDBAdmin]
--    EXEC sp_addrolemember N'db_owner', N'CLDBAdmin'
--END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User')
BEGIN
    CREATE TABLE [User] (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(255) NOT NULL,
        Surname NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        Password NVARCHAR(255) NOT NULL,
        ContactNumber NVARCHAR(20),
        Role INT NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Message')
BEGIN
    CREATE TABLE Message (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        SenderId UNIQUEIDENTIFIER NOT NULL,
        ReceiverId UNIQUEIDENTIFIER NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        StatusCode INT DEFAULT(-1),
        StatusMessage VARCHAR(MAX) DEFAULT(''),
        CONSTRAINT FK_Message_SenderId FOREIGN KEY (SenderId) REFERENCES [User](Id),
        CONSTRAINT FK_Message_ReceiverId FOREIGN KEY (ReceiverId) REFERENCES [User](Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(SenderId, ReceiverId)
    );
END
ELSE
BEGIN
        
        IF NOT EXISTS (SELECT * FROM syscolumns WHERE ID=OBJECT_ID('dbo.Message') AND NAME='StatusCode')
        BEGIN
            ALTER TABLE [dbo].[Message] ADD [StatusCode] INT DEFAULT(-1)
        END  
        
        IF NOT EXISTS (SELECT * FROM syscolumns WHERE ID=OBJECT_ID('dbo.Message') AND NAME='StatusMessage')
        BEGIN
            ALTER TABLE [dbo].[Message] ADD [StatusMessage] VARCHAR(MAX) DEFAULT('')
        END  
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Module')
BEGIN
    CREATE TABLE Module (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Code NVARCHAR(50) NOT NULL UNIQUE,
        Name NVARCHAR(255) NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserModule')
BEGIN
    CREATE TABLE UserModule (
        UserId UNIQUEIDENTIFIER NOT NULL,
        ModuleId UNIQUEIDENTIFIER NOT NULL,
        PRIMARY KEY (UserId, ModuleId),
        FOREIGN KEY (UserId) REFERENCES [User](Id),
        FOREIGN KEY (ModuleId) REFERENCES Module(Id),
        INDEX IX_UserModules (UserId, ModuleId)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Topic')
BEGIN
    CREATE TABLE Topic (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX),
        DateCreated DATETIME DEFAULT GETDATE(),
        ModuleId UNIQUEIDENTIFIER NOT NULL,
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (ModuleId) REFERENCES Module(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(ModuleId, CreatedByUserId)
    );
END;

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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Discussion')
BEGIN
    CREATE TABLE Discussion (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(255) NOT NULL,
        CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
        TopicId UNIQUEIDENTIFIER NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CreatedByUserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(CreatedByUserId, TopicId)
    );
END;

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
    );
END;

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
    );
END;

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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Question')
BEGIN
    CREATE TABLE Question (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        QuizId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        QuestionType INT NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        DateRemoved DATETIME NULL,
        RemovedByUserId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (QuizId) REFERENCES Quiz(Id),
        FOREIGN KEY (RemovedByUserId) REFERENCES [User](Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(QuizId, QuestionType)
    );
END;

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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuizAttempt')
BEGIN
	CREATE TABLE QuizAttempt (
		Id UNIQUEIDENTIFIER PRIMARY KEY,
		UserId UNIQUEIDENTIFIER NOT NULL,
		QuizId UNIQUEIDENTIFIER NOT NULL,
		DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
		AssignedByUserId UNIQUEIDENTIFIER NOT NULL,
		DateAttempted DATETIME NULL,
		AttemptDuration TIME NULL,
		FOREIGN KEY (UserId) REFERENCES [User](Id),
		FOREIGN KEY (QuizId) REFERENCES Quiz(Id),
		FOREIGN KEY (AssignedByUserId) REFERENCES [User](Id)
	);
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuizAttemptQuestionAnswer')
BEGIN
	CREATE TABLE QuizAttemptQuestionAnswer (
		Id UNIQUEIDENTIFIER PRIMARY KEY,
		QuizAttemptId UNIQUEIDENTIFIER NOT NULL,
		QuestionId UNIQUEIDENTIFIER NOT NULL,
		QuestionOptionId UNIQUEIDENTIFIER NOT NULL,
		IsCorrect BIT NOT NULL,
		FOREIGN KEY (QuizAttemptId) REFERENCES QuizAttempt(Id),
		FOREIGN KEY (QuestionId) REFERENCES Question(Id),
		FOREIGN KEY (QuestionOptionId) REFERENCES QuestionOption(Id)
	);
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TopicSubscription')
BEGIN
    CREATE TABLE TopicSubscription (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        TopicId UNIQUEIDENTIFIER NOT NULL,
        DateSubscribed DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES [User](Id),
        FOREIGN KEY (TopicId) REFERENCES Topic(Id),
        INDEX IX_DateSubscribed (DateSubscribed ASC) INCLUDE(UserId, TopicId)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TutorSubscription')
BEGIN
    CREATE TABLE TutorSubscription (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        TutorId UNIQUEIDENTIFIER NOT NULL,
        DateSubscribed DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES [User](Id),
        FOREIGN KEY (TutorId) REFERENCES [User](Id)
    );
END;
GO

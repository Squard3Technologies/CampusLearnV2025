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
    )
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
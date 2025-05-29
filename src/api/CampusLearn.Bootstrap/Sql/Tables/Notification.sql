IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Notification')
BEGIN
    CREATE TABLE Notification (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        SenderId UNIQUEIDENTIFIER NOT NULL,
        ReceiverId UNIQUEIDENTIFIER NOT NULL,
	    NotificationType INT NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        DateCreated DATETIME DEFAULT GETDATE(),
        StatusCode INT DEFAULT(-1),
        StatusMessage VARCHAR(MAX) DEFAULT(''),
        CONSTRAINT FK_Notification_SenderId FOREIGN KEY (SenderId) REFERENCES [User](Id),
        CONSTRAINT FK_Notification_ReceiverId FOREIGN KEY (ReceiverId) REFERENCES [User](Id),
        INDEX IX_DateCreated (DateCreated ASC) INCLUDE(SenderId, ReceiverId)
    )
END
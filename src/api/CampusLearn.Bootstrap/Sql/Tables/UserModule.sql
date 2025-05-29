
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserModule')
BEGIN
    CREATE TABLE UserModule (
        UserId UNIQUEIDENTIFIER NOT NULL,
        ModuleId UNIQUEIDENTIFIER NOT NULL,
        PRIMARY KEY (UserId, ModuleId),
        FOREIGN KEY (UserId) REFERENCES [User](Id),
        FOREIGN KEY (ModuleId) REFERENCES Module(Id),
        INDEX IX_UserModules (UserId, ModuleId)
    )
END
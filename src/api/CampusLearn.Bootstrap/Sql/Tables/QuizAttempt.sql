IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuizAttempt')
BEGIN
	CREATE TABLE QuizAttempt (
		Id UNIQUEIDENTIFIER PRIMARY KEY,
		UserId UNIQUEIDENTIFIER NOT NULL,
		QuizId UNIQUEIDENTIFIER NOT NULL,
		DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
		AssignedByUserId UNIQUEIDENTIFIER,
		DateAttempted DATETIME NULL,
		AttemptDuration TIME NULL,
		FOREIGN KEY (UserId) REFERENCES [User](Id),
		FOREIGN KEY (QuizId) REFERENCES Quiz(Id),
		FOREIGN KEY (AssignedByUserId) REFERENCES [User](Id)
	);
END;

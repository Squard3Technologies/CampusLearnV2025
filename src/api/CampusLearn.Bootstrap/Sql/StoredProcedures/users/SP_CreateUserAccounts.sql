CREATE OR ALTER PROCEDURE dbo.SP_CreateUserAccounts
(
    @Name NVARCHAR(255),
    @Middlename VARCHAR(255) = NULL,
    @Surname NVARCHAR(255),
    @Email NVARCHAR(255),
    @ContactNumber NVARCHAR(50),
    @Password NVARCHAR(MAX),
    @Role INT
)
AS
BEGIN
	DECLARE @Status BIT, @StatusCode INT, @StatusMessage VARCHAR(MAX)
	IF(EXISTS(SELECT * FROM dbo.[User] U WITH(NOLOCK) WHERE U.Email = @Email))
	BEGIN
		SET @Status = 0
		SET @StatusCode = 409
		SET @StatusMessage = 'There is already an account linked to the provided email address'		
	END
	ELSE
	BEGIN
		DECLARE @Id UNIQUEIDENTIFIER = NEWID()
	    INSERT INTO [User] (Id, Name, MiddleName, Surname, Email, Password, ContactNumber, Role)
	    VALUES (@Id, @Name, ISNULL(@Middlename,''), @Surname, @Email, @Password, @ContactNumber, @Role);
		SET @Status = 1
		SET @StatusCode = 200
		SET @StatusMessage = 'Successfully created user account'
	END	
	SELECT @Status AS [Status], @StatusCode AS [StatusCode], @StatusMessage AS [StatusMessage]
END
CREATE OR ALTER PROCEDURE dbo.SP_GetDiscussionComments
(
    @DiscussionId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT 
        c.Content 'Title',
        c.CreatedDate 'DateCreated',
        c.CreatedByUserId,
        u.Name 'CreatedByUserName',
        u.Surname 'CreatedByUserSurname',
        u.Email 'CreatedByUserEmail'
    FROM Comment c
        INNER JOIN [User] u ON c.CreatedByUserId = u.Id
    WHERE c.DiscussionId = @DiscussionId
    ORDER BY c.CreatedDate DESC;
END;
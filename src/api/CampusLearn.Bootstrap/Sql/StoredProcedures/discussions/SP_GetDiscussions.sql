CREATE OR ALTER PROCEDURE dbo.SP_GetDiscussions
(
    @TopicId UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT 
        d.Id,
        d.Title,
        d.Content,
        d.DateCreated,
        d.CreatedByUserId,
        u.Name 'CreatedByUserName',
        u.Surname 'CreatedByUserSurname',
        u.Email 'CreatedByUserEmail',
        c.Content 'LastComment',
        c.CreatedDate 'LastCommentDateCreated'
    FROM Discussion d
    INNER JOIN [User] u ON d.CreatedByUserId = u.Id
    OUTER APPLY (
        SELECT TOP 1 c.Content, c.CreatedDate
        FROM Comment c
        WHERE c.DiscussionId = d.Id
        ORDER BY c.CreatedDate DESC
    ) c
    WHERE d.TopicId = @TopicId
    ORDER BY d.DateCreated DESC;
END;
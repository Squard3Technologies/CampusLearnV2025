namespace CampusLearn.DataLayer.RepositoryService;

public class DiscussionRepository : IDiscussionRepository
{
    #region -- protected properties --

    protected readonly ILogger<DiscussionRepository> _logger;
    protected readonly CampusLearnDbContext _database;

    #endregion -- protected properties --

    public DiscussionRepository(ILogger<DiscussionRepository> logger, CampusLearnDbContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Guid?> CreateComment(Guid discussionId, Guid userId, CreateDiscussionCommentRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("DiscussionId", discussionId, DbType.Guid);
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Content", model.Content, DbType.String);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_CreateDiscussionComment",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<Guid?> CreateDiscussion(Guid topicId, Guid userId, CreateDiscussionRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("TopicId", topicId, DbType.Guid);
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Title", model.Title, DbType.String);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_CreateDiscussion",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<List<DiscussionCommentViewModel>> GetDiscussionComments(Guid discussionId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("DiscussionId", discussionId, DbType.Guid);

            var results = await db.QueryAsync<DiscussionCommentViewModel>(
                "dbo.SP_GetDiscussionComments",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }

    public async Task<List<DiscussionViewModel>> GetDiscussionsByTopic(Guid topicId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("TopicId", topicId, DbType.Guid);

            var results = await db.QueryAsync<DiscussionViewModel>(
                "dbo.SP_GetDiscussions",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }
}

namespace CampusLearn.DataLayer.RepositoryService;

public class ChatRepository : IChatRepository
{
    #region -- protected properties --

    protected readonly ILogger<ChatRepository> _logger;
    protected readonly CampusLearnDbContext _database;

    #endregion -- protected properties --

    public ChatRepository(ILogger<ChatRepository> logger, CampusLearnDbContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Guid?> CreateChatMessage(Guid sentByUserId, Guid sentToUserId, CreateChatMessageRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("SenderId", sentByUserId, DbType.Guid);
            parameters.Add("ReceiverId", sentToUserId, DbType.Guid);
            parameters.Add("Content", model.Content, DbType.String);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_CreateChatMessage",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<List<ChatMessageViewModel>> GetChatMessages(Guid sentByUserId, Guid sentToUserId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("SenderId", sentByUserId, DbType.Guid);
            parameters.Add("ReceiverId", sentToUserId, DbType.Guid);

            var results = await db.QueryAsync<ChatMessageViewModel>(
                "dbo.SP_GetChatMessages",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }

    public async Task<List<ChatViewModel>> GetChatsByUser(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var results = await db.QueryAsync<ChatViewModel>(
                "dbo.SP_GetChatsByUser",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }
}

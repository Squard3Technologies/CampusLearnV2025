namespace CampusLearn.DataLayer.RepositoryService;

public class SubscriptionRepository : ISubscriptionRepository
{
    #region -- protected properties --

    protected readonly ILogger<SubscriptionRepository> _logger;
    protected readonly CampusLearnDbContext _database;

    #endregion -- protected properties --

    public SubscriptionRepository(ILogger<SubscriptionRepository> logger, CampusLearnDbContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<List<TutorSubscriptionViewModel>> GetAvailableTutors(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var multipleResults = await db.QueryMultipleAsync(
                "dbo.SP_GetAvailableTutors",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            var tutorSubscriptions = (await multipleResults.ReadAsync<TutorSubscriptionViewModel>())?.ToList() ?? [];
            var tutorModules = (await multipleResults.ReadAsync<UserModuleViewModel>())?.ToList() ?? [];
            var modules = (await multipleResults.ReadAsync<ModuleViewModel>())?.ToList() ?? [];

            foreach (var tutorSubscription in tutorSubscriptions)
            {
                var moduleIds = tutorModules.Where(x => x.TutorId == tutorSubscription.TutorId).Select(x => x.ModuleId) ?? [];

                tutorSubscription.LinkedModules = modules.Where(x => moduleIds.Contains(x.Id)).ToList();
            }

            return tutorSubscriptions ?? [];
        });
    }

    public async Task<List<Guid>> GetSubscribedUserIdentifiers(Guid? userId, Guid? tutorId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("TutorId", tutorId, DbType.Guid);

            var results = await db.QueryAsync<Guid>(
                "dbo.SP_GetSubscribedUserIdentifiers",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }

    public async Task<List<TopicSubscriptionViewModel>> GetTopics(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var results = await db.QueryAsync<TopicSubscriptionViewModel>(
                "dbo.SP_GetSubscribedTopics",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }

    public async Task<List<TutorSubscriptionViewModel>> GetTutors(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var multipleResults = await db.QueryMultipleAsync(
                "dbo.SP_GetSubscribedTutors",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            var tutorSubscriptions = (await multipleResults.ReadAsync<TutorSubscriptionViewModel>())?.ToList() ?? [];
            var tutorModules = (await multipleResults.ReadAsync<UserModuleViewModel>())?.ToList() ?? [];
            var modules = (await multipleResults.ReadAsync<ModuleViewModel>())?.ToList() ?? [];

            foreach (var tutorSubscription in tutorSubscriptions)
            {
                var moduleIds = tutorModules.Where(x => x.TutorId == tutorSubscription.TutorId).Select(x => x.ModuleId) ?? [];

                tutorSubscription.LinkedModules = modules.Where(x => moduleIds.Contains(x.Id)).ToList();
            }

            return tutorSubscriptions ?? [];
        });
    }

    public async Task<Guid?> SubscribeToTopic(Guid userId, Guid topicId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("TopicId", topicId, DbType.Guid);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_SubscribeToTopic",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<Guid?> SubscribeToTutor(Guid userId, Guid tutorId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("TutorId", tutorId, DbType.Guid);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_SubscribeToTutor",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task UnsubscribeFromTopic(Guid userId, Guid topicId, CancellationToken token)
    {
        await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("TopicId", topicId, DbType.Guid);

            await db.ExecuteAsync(
                "dbo.SP_UnsubscribeFromTopic",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
    }

    public async Task UnsubscribeFromTutor(Guid userId, Guid tutorId, CancellationToken token)
    {
        await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("TutorId", tutorId, DbType.Guid);

            await db.ExecuteAsync(
                "dbo.SP_UnsubscribeFromTutor",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
    }
}

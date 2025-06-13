namespace CampusLearn.DataLayer.RepositoryService;

public class EnquiryRepository : IEnquiryRepository
{
    #region -- protected properties --

    protected readonly ILogger<EnquiryRepository> _logger;
    protected readonly CampusLearnDbContext _database;

    #endregion -- protected properties --

    public EnquiryRepository(ILogger<EnquiryRepository> logger, CampusLearnDbContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<List<EnquiryViewModel>> GetEnquiriesAsync(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var result = await db.QueryAsync<EnquiryViewModel>(
                "dbo.SP_EnquiriesByUser",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result?.ToList() ?? [];
        });
    }

    public async Task<Guid?> CreateEnquiryAsync(Guid userId, CreateEnquiryRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("CreatedByUserId", userId, DbType.Guid);
            parameters.Add("Title", model.Title, DbType.String);
            parameters.Add("Description", model.Description, DbType.String);
            parameters.Add("EnquiryStatus", EnquiryStatus.Pending, DbType.Int16);
            parameters.Add("ModuleId", model.ModuleId, DbType.Guid);

            var result = await db.QueryFirstOrDefaultAsync<Guid?>(
                "dbo.SP_CreateEnquiry",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<List<EnquiryViewModel>> GetEnquiriesByStatusAsync(EnquiryStatus statusFilter, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("Status", statusFilter, DbType.Int16);

            var result = await db.QueryAsync<EnquiryViewModel>(
                "dbo.SP_EnquiriesByStatus",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result?.ToList() ?? [];
        });
    }

    public async Task<EnquiryViewModel?> GetEnquiryAsync(Guid id, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Guid);

            var result = await db.QueryFirstOrDefaultAsync<EnquiryViewModel>(
                "dbo.SP_GetEnquiry",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<GenericDbResponseViewModel> ResolveEnquiryAsync(Guid enquiryId, Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("EnquiryId", enquiryId, DbType.Guid);
            parameters.Add("ResolvedByUserId", tutorId, DbType.Guid);
            parameters.Add("Status", EnquiryStatus.Resolved, DbType.Int16);
            parameters.Add("ResolutionAction", model.ResolutionAction, DbType.Int16);
            parameters.Add("ResolutionResponse", model.ResolutionResponse, DbType.String);
            parameters.Add("LinkedTopicId", model.LinkedTopicId, DbType.Guid);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel>(
                "dbo.SP_ResolveEnquiry",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }
}
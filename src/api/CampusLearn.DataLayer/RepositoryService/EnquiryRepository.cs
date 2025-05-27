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

    public async Task CreateEnquiryAsync(Guid userId, CreateEnquiryRequestModel model, CancellationToken token)
    {
        await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("CreatedByUserId", userId, DbType.Guid);
            parameters.Add("Title", model.Title, DbType.String);
            parameters.Add("Content", model.Description, DbType.String);
            parameters.Add("Status", EnquiryStatus.Pending, DbType.Int16);
            parameters.Add("ModuleId", model.ModuleId, DbType.Guid);

            var rowsAffected = await db.ExecuteAsync(
                "dbo.SP_CreateEnquiry",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
    }

    public Task<GenericDbResponseViewModel> GetEnquiriesByStatus(Guid userId, EnquiryStatus statusFilter, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<GenericDbResponseViewModel> GetEnquiryAsync(Guid userId, Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task ResolveEnquiry(Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}

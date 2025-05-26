namespace CampusLearn.DataLayer.RepositoryService;

public class MessagingRepository : IMessagingRepository
{
    protected readonly CampusLearnDbContext database;

    public MessagingRepository(CampusLearnDbContext database)
    {
        this.database = database;
    }

    public async Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, string messageType)
    {
        GenericDbResponseViewModel response = new GenericDbResponseViewModel();
        try
        {
            using (var db = database.CreateSqlConnection())
            {
                await db.OpenAsync();
                using (SqlTransaction sqltrans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string query = "dbo.SP_CreateMessage";

                        var parameters = new DynamicParameters();
                        parameters.Add("SenderId", model.SenderId, DbType.Guid);
                        parameters.Add("ReceiverId", model.RecieverId, DbType.Guid);
                        parameters.Add("MessageType", messageType, DbType.String);
                        parameters.Add("Content", model.MessageContent, DbType.String);

                        response = await db.QueryFirstAsync<GenericDbResponseViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);

                        await sqltrans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await sqltrans.RollbackAsync();
                        await db.CloseAsync();
                        throw ex;
                    }
                }
                await db.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.StatusCode = 500;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetAllPendingMessagesAsync(string messageType, int batchSize = 100)
    {
        GenericDbResponseViewModel response = new GenericDbResponseViewModel();
        try
        {
            using (var db = database.CreateSqlConnection())
            {
                await db.OpenAsync();
                using (SqlTransaction sqltrans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string query = "dbo.SP_GetPendingMessages";
                        var parameters = new DynamicParameters();
                        parameters.Add("messageType", messageType, DbType.String);
                        parameters.Add("batchSize", batchSize, DbType.Int16);

                        var dbResponse = await db.QueryAsync<MessageViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        if (dbResponse.Any())
                        {
                            response.Status = true;
                            response.Body = dbResponse.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = $"No pending messages found for message type: {messageType}";
                        }
                        await sqltrans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await sqltrans.RollbackAsync();
                        await db.CloseAsync();
                        throw ex;
                    }
                }
                await db.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.StatusCode = 500;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task UpdateMessageStatusAsync(MessageViewModel model, int statusCode, string statusMessage)
    {
        try
        {
            using (var db = database.CreateSqlConnection())
            {
                await db.OpenAsync();
                using (SqlTransaction sqltrans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string query = "dbo.SP_UpdateMessageStatus";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", model.ID, DbType.Guid);
                        parameters.Add("statusCode", statusCode, DbType.String);
                        parameters.Add("statusMessage", statusMessage, DbType.String);

                        await db.QueryAsync(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);

                        await sqltrans.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await sqltrans.RollbackAsync();
                        await db.CloseAsync();
                        throw ex;
                    }
                }
                await db.CloseAsync();
            }
        }
        catch (Exception ex)
        {
        }
    }
}
namespace CampusLearn.DataLayer.RepositoryService;

public class NotificationRepository : INotificationRepository
{
    protected readonly CampusLearnDbContext database;

    public NotificationRepository(CampusLearnDbContext database)
    {
        this.database = database;
    }

    public async Task<GenericDbResponseViewModel> CreateNotificationAsync(SendMessageRequest model, NotificationTypes notificationType)
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
                        string query = "dbo.SP_CreateNotification";

                        var parameters = new DynamicParameters();
                        parameters.Add("SenderId", model.SenderId, DbType.Guid);
                        parameters.Add("ReceiverId", model.RecieverId, DbType.Guid);
                        parameters.Add("NotificationType", notificationType, DbType.Int16);
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

    public async Task<GenericDbResponseViewModel> GetAllPendingNotificationAsync(NotificationTypes notificationType, int batchSize = 100)
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
                        string query = "dbo.SP_GetPendingNotifications";
                        var parameters = new DynamicParameters();
                        parameters.Add("notificationType", notificationType, DbType.Int16);
                        parameters.Add("batchSize", batchSize, DbType.Int16);

                        var dbResponse = await db.QueryAsync<NotificationViewModel>(sql: query,
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
                            response.StatusMessage = $"No pending messages found for message type: {Enum.GetName(notificationType)}";
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

    public async Task<string> GetEmailTemplate(NotificationContentTypes notificationContentType)
    {
        return notificationContentType switch
        {
            NotificationContentTypes.AccountApproved => FileExtensions.GetEmailTemplate("AccountApproved.html"),
            NotificationContentTypes.AccountRejected => FileExtensions.GetEmailTemplate("AccountRejected.html"),
            NotificationContentTypes.AccountDeactivated => FileExtensions.GetEmailTemplate("AccountDeactivated.html"),
            NotificationContentTypes.TopicCreated => FileExtensions.GetEmailTemplate("TopicCreated.html"),
            NotificationContentTypes.TopicQuizCreated => FileExtensions.GetEmailTemplate("TopicQuizCreated.html"),
            NotificationContentTypes.TopicQuizUpdated => FileExtensions.GetEmailTemplate("TopicQuizUpdated.html"),
            NotificationContentTypes.TopicDiscussionCreated => FileExtensions.GetEmailTemplate("TopicDiscussionCreated.html"),
            NotificationContentTypes.TopicCommentCreated => FileExtensions.GetEmailTemplate("TopicCommentCreated.html"),
            NotificationContentTypes.TopicLearningMaterialUploaded => FileExtensions.GetEmailTemplate("TopicLearningMaterialUploaded.html"),
            NotificationContentTypes.TopicQuizAssigned => FileExtensions.GetEmailTemplate("TopicQuizAssigned.html"),
            NotificationContentTypes.EnquiryCreated => FileExtensions.GetEmailTemplate("EnquiryCreated.html"),
            NotificationContentTypes.EnquiryResolved => FileExtensions.GetEmailTemplate("EnquiryResolved.html"),
            NotificationContentTypes.ChatMessageCreated => FileExtensions.GetEmailTemplate("ChatMessageCreated.html"),
            _ => "",
        };
    }

    public async Task UpdateNotificationStatusAsync(NotificationViewModel model, int statusCode, string statusMessage)
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
                        string query = "dbo.SP_UpdateNotificationStatus";
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

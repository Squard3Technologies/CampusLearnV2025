using System.Net;
using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.DataLayer.RepositoryService;

public class ModuleRepository : IModuleRepository
{
    #region -- protected properties --

    protected readonly ILogger<ModuleRepository> logger;
    protected readonly CampusLearnDbContext database;

    #endregion -- protected properties --

    public ModuleRepository(ILogger<ModuleRepository> logger, CampusLearnDbContext database)
    {
        this.logger = logger;
        this.database = database;
    }

    public async Task<GenericDbResponseViewModel<Guid?>> AddModuleAsync(ModuleViewModel module)
    {
        GenericDbResponseViewModel<Guid?> response = new GenericDbResponseViewModel<Guid?>();
        try
        {
            using (var db = database.CreateSqlConnection())
            {
                await db.OpenAsync();
                using (SqlTransaction sqltrans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string query = "dbo.SP_AddModule";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", module.Id, DbType.Guid);
                        parameters.Add("code", module.Code, DbType.String);
                        parameters.Add("name", module.Name, DbType.String);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel<Guid?>>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> UpdateModuleAsync(ModuleViewModel module)
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
                        string query = "dbo.SP_UpdateModule";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", module.Id, DbType.Guid);
                        parameters.Add("code", module.Code, DbType.String);
                        parameters.Add("name", module.Name, DbType.String);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> ChangeModuleStatusAsync(Guid moduleId, bool status)
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
                        string query = "dbo.SP_ChangeModuleStatus";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", moduleId, DbType.Guid);
                        parameters.Add("moduleStatus", status, DbType.Boolean);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> AddUserModuleAsync(Guid userId, Guid moduleId)
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
                        string query = "dbo.SP_AddUserModule";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);
                        parameters.Add("moduleId", moduleId, DbType.Guid);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetModulesAsync()
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
                        string query = "dbo.SP_GetModules";
                        var modules = await db.QueryAsync<ModuleViewModel>(sql: query,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (modules != null && modules?.Any() != false)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = modules.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "No modules found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetUserModulesAsync(Guid userId)
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
                        string query = "dbo.SP_GetUserModules";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);

                        var modules = await db.QueryAsync<ModuleViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (modules != null && modules?.Any() != false)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = modules.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "No modules found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetUserModuleLinksAsync(Guid userId)
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
                        string query = "dbo.SP_GetUserModuleLinks";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);
                        var userModules = await db.QueryAsync<UsersModuleViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (userModules != null && userModules?.Any() != false)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = userModules.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "No modules linked to this user in the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    #region -- topic section --

    public async Task<GenericDbResponseViewModel<Guid?>> AddTopicAsync(Guid userId, Guid moduleId, CreateTopicRequest module)
    {
        GenericDbResponseViewModel<Guid?> response = new GenericDbResponseViewModel<Guid?>();
        try
        {
            using (var db = database.CreateSqlConnection())
            {
                await db.OpenAsync();
                using (SqlTransaction sqltrans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string query = "dbo.SP_CreateTopic";
                        var parameters = new DynamicParameters();
                        parameters.Add("authorId", userId, DbType.Guid);
                        parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                        parameters.Add("moduleId", moduleId, DbType.Guid);
                        parameters.Add("title", module.Title, DbType.String);
                        parameters.Add("description", module.Description, DbType.String);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel<Guid?>>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetModuleTopicsAsync(Guid moduleId)
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
                        string query = "dbo.SP_GetModuleTopics";
                        var parameters = new DynamicParameters();
                        parameters.Add("moduleId", moduleId, DbType.Guid);
                        var topics = await db.QueryAsync<TopicViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (topics?.Any() == true)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = topics.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "There are no topics for this module found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetModuleTopicAsync(Guid moduleId, Guid topicId)
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
                        string query = "dbo.SP_GetModuleTopic";
                        var parameters = new DynamicParameters();
                        parameters.Add("moduleId", moduleId, DbType.Guid);
                        parameters.Add("topicId", topicId, DbType.Guid);
                        var topic = await db.QueryFirstOrDefaultAsync<TopicViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (topic != null)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = topic;
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "There are no topics for this module found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    #endregion -- topic section --

    #region -- learing material section --

    public async Task<GenericDbResponseViewModel> AddLearningMaterialAsync(LearningMaterialViewModel model)
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
                        string query = "dbo.SP_AddLearningMaterial";
                        var parameters = new DynamicParameters();
                        parameters.Add("id", model.Id, DbType.Guid);
                        parameters.Add("uploadedByUserId", model.UserId, DbType.Guid);
                        parameters.Add("topicId", model.TopicId, DbType.Guid);
                        parameters.Add("fileType", model.FileType, DbType.String);
                        parameters.Add("filePath", model.FilePath, DbType.String);
                        response = await db.QueryFirstOrDefaultAsync<GenericDbResponseViewModel>(sql: query,
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetUserLearningMaterialAsync(Guid userId)
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
                        string query = "dbo.SP_GetModuleTopics";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);
                        var materials = await db.QueryAsync<LearningMaterialViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (materials?.Any() == true)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = materials.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "There are no learning material for the user found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetUserTopicLearningMaterialAsync(Guid userId, Guid topicId)
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
                        string query = "dbo.SP_GetModuleTopics";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);
                        parameters.Add("topicId", topicId, DbType.Guid);
                        var materials = await db.QueryAsync<LearningMaterialViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (materials?.Any() == true)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = materials.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "There are no topics for this module found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    public async Task<GenericDbResponseViewModel> GetTopicLearningMaterialAsync(Guid topicId)
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
                        string query = "dbo.SP_GetModuleTopics";
                        var parameters = new DynamicParameters();
                        parameters.Add("topicId", topicId, DbType.Guid);
                        var materials = await db.QueryAsync<LearningMaterialViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        await sqltrans.CommitAsync();

                        if (materials?.Any() == true)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = materials.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "There are no topics for this module found on the system";
                        }
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }

    #endregion -- learing material section --
}
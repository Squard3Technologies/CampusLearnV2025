using CampusLearn.DataModel.Models.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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


    public async Task<GenericDbResponseViewModel> AddModuleAsync(ModuleViewModel module)
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
                        string query = "dbo.SP_AddModule";
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

    public async Task<GenericDbResponseViewModel> AddTopicAsync(Guid userId, CreateTopicRequest module)
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
                        string query = "dbo.SP_CreateTopic";
                        var parameters = new DynamicParameters();
                        parameters.Add("authorId", userId, DbType.Guid);
                        parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                        parameters.Add("moduleId", module.ModuleId, DbType.Guid);
                        parameters.Add("title", module.Title, DbType.String);
                        parameters.Add("description", module.Description, DbType.String);
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



    public async Task<GenericDbResponseViewModel> GetModuleTopicAsync(Guid moduleId)
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



    #endregion

}

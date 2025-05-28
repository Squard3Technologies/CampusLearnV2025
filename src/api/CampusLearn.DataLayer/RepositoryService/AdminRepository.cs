using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.RepositoryService;

public class AdminRepository : IAdminRepository
{
    #region -- protected properties --

    protected readonly ILogger<AdminRepository> logger;
    protected readonly CampusLearnDbContext database;

    #endregion -- protected properties --

    public AdminRepository(ILogger<AdminRepository> logger, CampusLearnDbContext database)
    {
        this.logger = logger;
        this.database = database;
    }


    public async Task<GenericDbResponseViewModel> GetPendingRegistrationsAsync()
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
                        string query = "dbo.SP_GetPendingRegistrations";
                        var registrations = await db.QueryAsync<UserViewModel>(sql: query,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        if (registrations != null && registrations?.Any() != false)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = registrations.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "No pending registrations found on the system";
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }



    public async Task<GenericDbResponseViewModel> ChangeUserAccountStatusAsync(Guid userId, Guid accountStatusId)
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
                        string query = "dbo.SP_ChangeAccountStatus";
                        var parameters = new DynamicParameters();
                        parameters.Add("userId", userId, DbType.Guid);
                        parameters.Add("accountStatusId", accountStatusId, DbType.Guid);
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


    public async Task<GenericDbResponseViewModel> GetUsersAsync()
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
                        string query = "dbo.SP_GetUsers";
                        var registrations = await db.QueryAsync<UserViewModel>(sql: query,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        if (registrations != null)
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = registrations.ToList();
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = 404;
                            response.StatusMessage = "No active users found on the system";
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}";
        }
        return response;
    }
}

using CampusLearn.DataLayer.DbContext;
using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models.User;
using CampusLearn.DataModel.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CampusLearn.DataLayer.RepositoryService;

public class UserRepository : IUserRepository
{
    #region -- protected properties --
    protected readonly ILogger<UserRepository> logger;
    protected readonly CampusLearnDbContext database;
    #endregion

    public UserRepository(ILogger<UserRepository> logger, CampusLearnDbContext database)
    {
        this.logger = logger;
        this.database = database;
    }

    public async Task<GenericDbResponseViewModel> ChangeUserPasswordAsync(Guid userId, string password)
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
                        string query = "dbo.SP_UserLogin";
                        var parameters = new DynamicParameters();
                        //parameters.Add("Name", batchSize, DbType.String);

                        //response = await db.QueryAsync<MessageViewModel>(sql: query,
                        //    param: parameters,
                        //    commandType: CommandType.StoredProcedure,
                        //    commandTimeout: 360,
                        //    transaction: sqltrans);

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
        return response;
    }


    public async Task<GenericDbResponseViewModel> CreateUserAccountAsync(CreateUserRequestModel user)
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
                        string query = "dbo.SP_CreateUserAccounts";
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", user.FirstName, DbType.String);
                        parameters.Add("Surname", user.Surname, DbType.String);
                        parameters.Add("Email", user.EmailAddress, DbType.String);
                        parameters.Add("ContactNumber", user.ContactNumber, DbType.String);
                        parameters.Add("Password", user.Password, DbType.String);
                        parameters.Add("Role", user.Role, DbType.Int16);

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

    public async Task<GenericDbResponseViewModel> LoginAsync(string emailAddress)
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
                        string query = "dbo.SP_UserLogin";
                        var parameters = new DynamicParameters();
                        parameters.Add("EmailAddress", emailAddress, DbType.String);

                        var dbUser = await db.QueryFirstOrDefaultAsync<UserViewModel>(sql: query,
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        if(dbUser != null)
                        {
                            response.Body = dbUser;
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

        }
        return response;
    }
}

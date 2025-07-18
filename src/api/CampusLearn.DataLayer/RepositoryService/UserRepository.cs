﻿namespace CampusLearn.DataLayer.RepositoryService;

public class UserRepository : IUserRepository
{
    #region -- protected properties --

    protected readonly ILogger<UserRepository> logger;
    protected readonly CampusLearnDbContext database;

    #endregion -- protected properties --

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
                        if (dbUser != null)
                        {
                            if (dbUser.AccountStatusId == AccountStatus.INACTIVE)
                            {
                                response.Status = false;
                                response.StatusCode = 404;
                                response.StatusMessage = "Account is inactive";
                            }
                            else if (dbUser.AccountStatusId == AccountStatus.LOCKED)
                            {
                                response.Status = false;
                                response.StatusCode = 404;
                                response.StatusMessage = "Account not locked";
                            }
                            else if (dbUser.AccountStatusId == AccountStatus.PENDING)
                            {
                                response.Status = false;
                                response.StatusCode = 404;
                                response.StatusMessage = "Account pending activation by administrator";
                            }
                            else if (dbUser.AccountStatusId == AccountStatus.REJECTED)
                            {
                                response.Status = false;
                                response.StatusCode = 404;
                                response.StatusMessage = "Account registration rejected by administrator";
                            }
                            else
                            {
                                response.Status = true;
                                response.StatusCode = 200;
                                response.StatusMessage = "Login successful";
                                response.Body = dbUser;
                            }
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

    public async Task<UserProfileViewModel?> GetUserProfileAsync(Guid userId, CancellationToken token)
    {
        return await database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var multipleResults = await db.QueryMultipleAsync(
                "dbo.SP_GetUserProfile",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            var userProfile = await multipleResults.ReadFirstOrDefaultAsync<UserProfileViewModel>();
            if (userProfile == null)
                return null;

            userProfile.LinkedModules = (await multipleResults.ReadAsync<UserProfileModuleViewModel>()).ToList();

            return userProfile;
        });
    }

    public async Task UpdateUserProfileAsync(Guid userId, UserProfileRequestModel model, CancellationToken token)
    {
        await database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Name", model.Name, DbType.String);
            parameters.Add("Surname", model.Surname, DbType.String);
            parameters.Add("ContactNumber", model.ContactNumber, DbType.String);

            await db.ExecuteAsync(
                "dbo.SP_UpdateUserProfile",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
    }

    public async Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken token)
    {
        await database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Password", newPassword, DbType.String);

            await db.ExecuteAsync(
                "dbo.SP_ChangePassword",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
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
                        string query = "dbo.SP_GetAllActiveUsers";
                        var activeUsers = await db.QueryAsync<UserViewModel>(sql: query,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 360,
                            transaction: sqltrans);
                        if (activeUsers != null && activeUsers.Any())
                        {
                            response.Status = true;
                            response.StatusCode = 200;
                            response.StatusMessage = "Successful";
                            response.Body = activeUsers.ToList();
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
        }
        return response;
    }
}
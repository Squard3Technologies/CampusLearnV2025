using CampusLearn.DataLayer.DbContext;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CampusLearn.Bootstrap
{
    public class Bootstraper
    {
        #region -- protected properties --

        protected readonly ILogger<Bootstraper> logger;
        protected readonly CampusLearnDbContext database;

        #endregion -- protected properties --

        public Bootstraper(ILogger<Bootstraper> logger, CampusLearnDbContext database)
        {
            this.logger = logger;
            this.database = database;
        }


        public async Task Migrations()
        {

            await MigrateStoredProceduresAsync();
        }




        #region -- protected functions --

        


        private async Task MigrateStoredProceduresAsync()
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "StoredProcedures");
                using(var db = database.CreateSqlConnection())
                {
                    await db.OpenAsync();
                    string sqlQuery = "";

                    #region -- SP_UserLogin --
                    sqlQuery = GetSqlScript(basePath, "SP_UserLogin.sql"); 
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_UserLogin.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_ChangeAccountStatus --
                    sqlQuery = GetSqlScript(basePath, "SP_ChangeAccountStatus.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_ChangeAccountStatus.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_CreateUserAccounts --
                    sqlQuery = GetSqlScript(basePath, "SP_CreateUserAccounts.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_CreateUserAccounts.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_CreateMessage --
                    sqlQuery = GetSqlScript(basePath, "SP_CreateMessage.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_CreateMessage.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_GetPendingMessages --
                    sqlQuery = GetSqlScript(basePath, "SP_GetPendingMessages.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_GetPendingMessages.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_UpdateMessageStatus --
                    sqlQuery = GetSqlScript(basePath, "SP_UpdateMessageStatus.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_UpdateMessageStatus.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_GetPendingRegistrations --
                    sqlQuery = GetSqlScript(basePath, "SP_GetPendingRegistrations.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_GetPendingRegistrations.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- SP_GetUsers --
                    sqlQuery = GetSqlScript(basePath, "SP_GetUsers.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find SP_GetUsers.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    await db.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }






        public string GetSqlScript(string folder, string filename)
        {
            var filePath = Path.Combine(folder, filename);
            if (!File.Exists(filePath))
                return string.Empty;

            return File.ReadAllText(filePath);
        }
        #endregion


    }
}

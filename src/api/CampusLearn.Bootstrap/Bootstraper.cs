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

            await MigrateTablesAsync();
            await MigrateTableSeedersAsync();
            await MigrateStoredProceduresAsync();
        }




        #region -- protected functions --


        private async Task MigrateTablesAsync()
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables");
                using (var db = database.CreateSqlConnection())
                {
                    await db.OpenAsync();
                    string sqlQuery = "";

                    #region -- Statuses --
                    sqlQuery = GetSqlScript(basePath, "Statuses.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Statuses.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- User --
                    sqlQuery = GetSqlScript(basePath, "User.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find User.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Message --
                    sqlQuery = GetSqlScript(basePath, "Message.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Message.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Module --
                    sqlQuery = GetSqlScript(basePath, "Module.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Module.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- UserModule --
                    sqlQuery = GetSqlScript(basePath, "UserModule.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find UserModule.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Topic --
                    sqlQuery = GetSqlScript(basePath, "Topic.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Topic.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Enquiry --
                    sqlQuery = GetSqlScript(basePath, "Enquiry.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Enquiry.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Discussion --
                    sqlQuery = GetSqlScript(basePath, "Discussion.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Discussion.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Comment --
                    sqlQuery = GetSqlScript(basePath, "Comment.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Comment.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- LearningMaterial --
                    sqlQuery = GetSqlScript(basePath, "LearningMaterial.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find LearningMaterial.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Quiz --
                    sqlQuery = GetSqlScript(basePath, "Quiz.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Quiz.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- Question --
                    sqlQuery = GetSqlScript(basePath, "Question.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find Question.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- QuestionOption --
                    sqlQuery = GetSqlScript(basePath, "QuestionOption.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find QuestionOption.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- QuizAttempt --
                    sqlQuery = GetSqlScript(basePath, "QuizAttempt.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find QuizAttempt.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- QuizAttemptQuestionAnswer --
                    sqlQuery = GetSqlScript(basePath, "QuizAttemptQuestionAnswer.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find QuizAttemptQuestionAnswer.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- TopicSubscription --
                    sqlQuery = GetSqlScript(basePath, "TopicSubscription.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find TopicSubscription.sql");
                    await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                    #endregion


                    #region -- TutorSubscription --
                    sqlQuery = GetSqlScript(basePath, "TutorSubscription.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find TutorSubscription.sql");
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


        private async Task MigrateTableSeedersAsync()
        {
            try
            {
                var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Seeders");
                using (var db = database.CreateSqlConnection())
                {
                    await db.OpenAsync();
                    string sqlQuery = "";

                    #region -- TableSeeders --
                    sqlQuery = GetSqlScript(basePath, "TableSeeders.sql");
                    if (string.IsNullOrEmpty(sqlQuery))
                        throw new Exception(@"Could not find TableSeeders.sql");
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

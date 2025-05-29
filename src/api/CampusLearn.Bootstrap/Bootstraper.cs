using CampusLearn.DataLayer.DbContext;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CampusLearn.Bootstrap;

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
        await database.CreateDatabase();
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

                #endregion -- Statuses --

                #region -- User --

                sqlQuery = GetSqlScript(basePath, "User.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find User.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- User --

                #region -- Message --

                sqlQuery = GetSqlScript(basePath, "Message.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Message.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Message --

                #region -- Module --

                sqlQuery = GetSqlScript(basePath, "Module.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Module.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Module --

                #region -- UserModule --

                sqlQuery = GetSqlScript(basePath, "UserModule.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find UserModule.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- UserModule --

                #region -- Topic --

                sqlQuery = GetSqlScript(basePath, "Topic.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Topic.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Topic --

                #region -- Enquiry --

                sqlQuery = GetSqlScript(basePath, "Enquiry.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Enquiry.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Enquiry --

                #region -- Discussion --

                sqlQuery = GetSqlScript(basePath, "Discussion.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Discussion.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Discussion --

                #region -- Comment --

                sqlQuery = GetSqlScript(basePath, "Comment.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Comment.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Comment --

                #region -- LearningMaterial --

                sqlQuery = GetSqlScript(basePath, "LearningMaterial.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find LearningMaterial.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- LearningMaterial --

                #region -- Quiz --

                sqlQuery = GetSqlScript(basePath, "Quiz.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Quiz.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Quiz --

                #region -- Question --

                sqlQuery = GetSqlScript(basePath, "Question.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Question.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Question --

                #region -- QuestionOption --

                sqlQuery = GetSqlScript(basePath, "QuestionOption.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find QuestionOption.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- QuestionOption --

                #region -- QuizAttempt --

                sqlQuery = GetSqlScript(basePath, "QuizAttempt.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find QuizAttempt.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- QuizAttempt --

                #region -- QuizAttemptQuestionAnswer --

                sqlQuery = GetSqlScript(basePath, "QuizAttemptQuestionAnswer.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find QuizAttemptQuestionAnswer.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- QuizAttemptQuestionAnswer --

                #region -- TopicSubscription --

                sqlQuery = GetSqlScript(basePath, "TopicSubscription.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find TopicSubscription.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- TopicSubscription --

                #region -- TutorSubscription --

                sqlQuery = GetSqlScript(basePath, "TutorSubscription.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find TutorSubscription.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- TutorSubscription --

                #region -- Notification --

                sqlQuery = GetSqlScript(basePath, "Notification.sql");
                if (string.IsNullOrEmpty(sqlQuery))
                    throw new Exception(@"Could not find Notification.sql");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);

                #endregion -- Notification --

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

                #endregion -- TableSeeders --

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
        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "StoredProcedures");

        if (!Directory.Exists(basePath))
        {
            Console.WriteLine($"[ERROR] SQL folder not found: {basePath}");
            throw new DirectoryNotFoundException($"SQL folder not found: {basePath}");
        }

        var sqlFiles = Directory.GetFiles(basePath, "*.sql", SearchOption.AllDirectories);

        Console.WriteLine($"[INFO] Starting stored procedure migration... Found {sqlFiles.Length} file(s).");

        using var db = database.CreateSqlConnection();
        await db.OpenAsync();

        foreach (var file in sqlFiles)
        {
            var fileName = Path.GetFileName(file);
            var sqlQuery = await File.ReadAllTextAsync(file);

            if (string.IsNullOrWhiteSpace(sqlQuery) || sqlQuery.TrimStart().StartsWith("--"))
            {
                Console.WriteLine($"[SKIP] Skipping empty or comment-only file: {fileName}");
                continue;
            }

            try
            {
                Console.WriteLine($"[EXEC] Executing {fileName}...");
                await db.ExecuteAsync(sql: sqlQuery, commandTimeout: 0, commandType: System.Data.CommandType.Text);
                Console.WriteLine($"[DONE] Successfully executed {fileName}");
            }
            catch (Exception fileEx)
            {
                Console.WriteLine($"[ERROR] Failed to execute {fileName}: {fileEx.Message}");
                throw new Exception($"Error executing SQL file: {file}", fileEx);
            }
        }

        await db.CloseAsync();
        Console.WriteLine("[SUCCESS] Stored procedure migration completed.");
    }

    public string GetSqlScript(string folder, string filename)
    {
        var filePath = Path.Combine(folder, filename);
        if (!File.Exists(filePath))
            return string.Empty;

        return File.ReadAllText(filePath);
    }

    #endregion -- protected functions --
}
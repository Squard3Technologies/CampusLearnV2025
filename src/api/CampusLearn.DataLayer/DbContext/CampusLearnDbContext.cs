namespace CampusLearn.DataLayer.DbContext;

public class CampusLearnDbContext
{
    #region -- protected readonly properties --

    protected readonly IConfiguration config;

    #endregion -- protected readonly properties --

    public CampusLearnDbContext(IConfiguration config)
    {
        this.config = config;
    }

    public async Task CreateDatabase()
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Database connection string not configured in the configuration file.");
        }

        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        // Build connection to master
        var masterConnectionString = connectionString.Replace(databaseName, "master");

        using (var connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            var checkDbExistsCmd = $@"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}')
            BEGIN
                CREATE DATABASE [{databaseName}]
            END";

            using (var command = new SqlCommand(checkDbExistsCmd, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public SqlConnection CreateSqlConnection()
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Database connection string not configured in the configuration file.");
        }
        else
        {
            return new SqlConnection(connectionString);
        }
    }
}

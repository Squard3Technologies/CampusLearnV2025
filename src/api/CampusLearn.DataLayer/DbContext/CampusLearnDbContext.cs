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
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CampusLearn.DataLayer.DbContext;

public class CampusLearnDbContext
{
    #region -- protected readonly properties --

    protected readonly IConfiguration config;

    #endregion


    public CampusLearnDbContext(IConfiguration config)
    {
        this.config = config;
    }


    public SqlConnection CreateSqlConnection()
    {
        if (string.IsNullOrEmpty(config.GetConnectionString("DefaultConnection")))
        {
            throw new Exception("Database connection string not configured in the configuration file.");
        }
        else
        {
            return new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }
    }


}

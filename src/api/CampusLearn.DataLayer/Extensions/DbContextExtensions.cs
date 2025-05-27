namespace CampusLearn.DataLayer.Extensions;

public static class DbContextExtensions
{
    public static async Task<T> ExecuteInTransactionAsync<T>(this CampusLearnDbContext dbContext, Func<SqlConnection, SqlTransaction, Task<T>> dbAction)
    {
        using (var db = dbContext.CreateSqlConnection())
        {
            await db.OpenAsync();
            using (var transaction = db.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var result = await dbAction(db, transaction);
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    await db.CloseAsync();
                }
            }
        }
    }

    public static async Task ExecuteInTransactionAsync(this CampusLearnDbContext dbContext, Func<SqlConnection, SqlTransaction, Task> dbAction)
    {
        using (var db = dbContext.CreateSqlConnection())
        {
            await db.OpenAsync();
            using (var transaction = db.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    await dbAction(db, transaction);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    await db.CloseAsync();
                }
            }
        }
    }
}

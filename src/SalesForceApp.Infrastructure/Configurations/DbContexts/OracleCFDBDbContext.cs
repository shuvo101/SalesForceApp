using Lib.DBAccess.Contexts;

namespace SalesForceApp.Infrastructure.Configurations.DbContexts;

public class OracleCFDBDbContext : OracleDbContext
{
    public OracleCFDBDbContext(string connectionString) : base(connectionString)
    {
    }
}

using Lib.DBAccess.Contexts;

namespace SalesForceApp.Infrastructure.Configurations.DbContexts;

public class OracleDMSPhaseFourDbContext : OracleDbContext
{
    public OracleDMSPhaseFourDbContext(string connectionString) : base(connectionString)
    {
    }
}

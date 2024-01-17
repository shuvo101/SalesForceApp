using SalesForceApp.Infrastructure.Configurations.DbContexts;
using SalesForceApp.Infrastructure.Configurations.Settings;

using Lib.DBAccess.Contexts;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Infrastructure.Configurations.Database;

internal static class DatabaseConfiguration
{
    internal static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = DatabaseSettings.Get(configuration);

        services.AddScoped(options => new OracleCFDBDbContext(databaseSettings.OracleCFDB));
        services.AddScoped(options => new OracleDMSPhaseFourDbContext(databaseSettings.OracleDMSPhaseFour));
        services.AddScoped(options => new MySqlDbContext(databaseSettings.MySql));
    }
}

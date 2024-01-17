using SalesForceApp.Core.Configurations.Injector;

using SalesForceApp.Infrastructure.Configurations.Database;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Infrastructure.Configurations.ServiceConfigurations;

public class InfrastructureConfigurations : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
    }
}

using SalesForceApp.Core.Configurations.Injector;
using SalesForceApp.Core.Configurations.UnitOfWorks;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Configurations.ServiceConfigurations;

public class CoreConfigurations : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services
            .AddHelperConfiguration()
            .AddMapperConfigurations();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}

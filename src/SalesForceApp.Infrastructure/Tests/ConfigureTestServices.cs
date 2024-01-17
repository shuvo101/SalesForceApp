using SalesForceApp.Core.Configurations.Injector;
using SalesForceApp.Core.Tests.Repository;

using SalesForceApp.Infrastructure.Tests;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Infrastructure.Users;

public class ConfigureTestServices : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddScoped<ITestRepository, TestRepository>();
    }
}

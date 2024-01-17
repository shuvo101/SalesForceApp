using SalesForceApp.Core.Configurations.Injector;
using SalesForceApp.Core.Tests.Service;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Tests;

public class ConfigureTestService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddScoped<TestService>();
    }
}

using SalesForceApp.Core.Configurations.Injector;
using SalesForceApp.Core.Users.Repository;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Infrastructure.Users;

public class ConfigureUserServices : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}

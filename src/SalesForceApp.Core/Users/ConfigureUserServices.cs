using SalesForceApp.Core.Configurations.Injector;
using SalesForceApp.Core.Users.Model;
using SalesForceApp.Core.Users.Service;
using SalesForceApp.Core.Users.Validators;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Users;

internal class ConfigureUserServices : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddScoped<UserService>();

        services.AddScoped<IValidator<UserLoginRequestModel>, UserLoginRequestModelValidator>();
    }
}

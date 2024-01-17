using SalesForceApp.Core.Configurations.Helpers;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Configurations.ServiceConfigurations;

public static class HelperConfiguration
{
    public static IServiceCollection AddHelperConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeHelper, DateTimeHelper>();

        return services;
    }
}

using SalesForceApp.Api.Configurations.Helpers;

using SalesForceApp.Core.Configurations.Helpers;
using SalesForceApp.Core.Configurations.Settings;

namespace SalesForceApp.Api.Configurations.ServiceConfigurations;

public static class HelperConfiguration
{
    public static void AddHelperConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenHelper, JwtTokenHelper>();
        services.AddScoped<FileHelper>();
        services.AddSingleton<ISettingsHelper, SettingsHelper>();
    }
}

using SalesForceApp.Api.Configurations.Exceptions;
using SalesForceApp.Api.Configurations.Middleware;
using SalesForceApp.Api.Configurations.ServiceInjectors;
using SalesForceApp.Core.Configurations.Constants;

namespace SalesForceApp.Api.Configurations.ServiceConfigurations;

public static class ApiConfigurations
{
    public static IServiceCollection AddApiConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInjectServices()
            .AddInjectServicesWithConfiguration(configuration)
            .AddSwaggerConfiguration()
            .AddEndpointConfiguration()
            .AddAuthConfiguration(configuration)
            .AddSettingsConfiguration(configuration)
            .AddHelperConfiguration();

        services.AddHttpClient(
            HttpClientKey.JsonPlaceHolder,
        client => client.BaseAddress = new Uri(configuration.GetValue<string>("ApiUrls:JsonPlaceHolder") ?? throw new Exception("ApiUrls:JsonPlaceHolder could not be loaded")));

        // Exception Handlers
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // Middleware
        services.AddTransient<LogRequestResponseMiddleware>();

        return services;
    }
}

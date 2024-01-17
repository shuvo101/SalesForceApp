using Mapster;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Configurations.ServiceConfigurations;

public static class MapperConfigurations
{
    public static IServiceCollection AddMapperConfigurations(this IServiceCollection services)
    {
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(Assembly.Load(typeof(ICoreAssemblyMarker).Assembly.FullName ?? throw new Exception("Couldn't load assembly for mapster.")));

        return services;
    }
}

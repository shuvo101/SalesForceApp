using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Configurations.Injector;

public interface IInjectServices
{
    void Configure(IServiceCollection services);
}

using Microsoft.Extensions.DependencyInjection;

namespace SalesForceApp.Core.Configurations.UnitOfWorks;

public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public T Repository<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}

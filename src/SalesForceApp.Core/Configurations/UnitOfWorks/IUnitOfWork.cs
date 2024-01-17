namespace SalesForceApp.Core.Configurations.UnitOfWorks;

public interface IUnitOfWork
{
    T Repository<T>() where T : notnull;
}

namespace SalesForceApp.Core.Configurations.Helpers;

public interface IDateTimeHelper
{
    public DateTime UtcNow { get; }
    public DateTime Now { get; }
}

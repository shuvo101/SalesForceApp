
namespace SalesForceApp.Core.Configurations.Helpers;

public class DateTimeHelper : IDateTimeHelper
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}

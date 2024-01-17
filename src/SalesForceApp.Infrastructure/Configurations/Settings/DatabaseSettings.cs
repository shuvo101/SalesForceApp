using Microsoft.Extensions.Configuration;

namespace SalesForceApp.Infrastructure.Configurations.Settings;

public class DatabaseSettings
{
    public const string SectionName = "Database";

    public string OracleCFDB { get; private set; } = string.Empty;
    public string OracleDMSPhaseFour { get; private set; } = string.Empty;
    public string MySql { get; private set; } = string.Empty;

    public static DatabaseSettings Get(IConfiguration configuration)
    {
        return new DatabaseSettings
        {
            OracleCFDB = configuration[$"{SectionName}:{nameof(OracleCFDB)}"] ?? string.Empty,
            OracleDMSPhaseFour = configuration[$"{SectionName}:{nameof(OracleDMSPhaseFour)}"] ?? string.Empty,
            MySql = configuration[$"{SectionName}:{nameof(MySql)}"] ?? string.Empty,
        };
    }
}

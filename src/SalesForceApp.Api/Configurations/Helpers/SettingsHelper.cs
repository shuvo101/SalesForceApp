using SalesForceApp.Core.Configurations.Settings;

namespace SalesForceApp.Api.Configurations.Helpers;

public class SettingsHelper : ISettingsHelper
{
    private readonly IConfiguration _configuration;

    public SettingsHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public T Get<T>(string sectionName) where T : ISettings
    {
        var section = _configuration.GetSection(sectionName);

        return section.Get<T>() ?? throw new Exception($"Could not load section: {sectionName} from appsettings");
    }
}

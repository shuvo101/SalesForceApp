namespace SalesForceApp.Core.Configurations.Settings;

public interface ISettingsHelper
{
    public T Get<T>(string sectionName) where T : ISettings;
}

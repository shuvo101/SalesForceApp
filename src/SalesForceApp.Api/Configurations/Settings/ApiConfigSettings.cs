namespace SalesForceApp.Api.Configurations.Settings;

public class ApiConfigSettings
{
    public const string SectionName = "ApiConfig";

    public bool UseHttps { get; set; }
    public SwaggerBaseUrl[] SwaggerBaseUrlList { get; set; } = [];

    public static ApiConfigSettings Get(IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);

        return section.Get<ApiConfigSettings>() ?? throw new Exception($"Could not load section: {SectionName} from appsettings");
    }
}

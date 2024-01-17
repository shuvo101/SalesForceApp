namespace SalesForceApp.Core.Configurations.Settings;

public class JwtSettings
{
    public const string SectionName = "JWT";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpireInMinutes { get; set; }
    public int RefreshTokenExpireInHourIfNotRememberMe { get; set; }
    public int RefreshTokenExpireInHourIfRememberMe { get; set; }
    public int TimeBeforeRefreshTokenExpirationInHour { get; set; }
}

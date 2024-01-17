using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using SalesForceApp.Core.Configurations.Helpers;
using SalesForceApp.Core.Configurations.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SalesForceApp.Api.Configurations.Helpers;

public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly IOptions<JwtSettings> _jwtSettingsOption;
    private readonly IDateTimeHelper _dateTimeHelper;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettingsOption, IDateTimeHelper dateTimeHelper)
    {
        _jwtSettingsOption = jwtSettingsOption;
        _dateTimeHelper = dateTimeHelper;
    }

    public JwtSettings JwtSettings => _jwtSettingsOption.Value;

    public Ulid GenerateNewRefreshToken()
    {
        return Ulid.NewUlid();
    }

    public string GenerateNewToken(IEnumerable<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var jwtSettings = _jwtSettingsOption.Value;

        var jwtKey = jwtSettings.Key;
        var jwtIssuer = jwtSettings.Issuer;
        var jwtAudience = jwtSettings.Audience;
        var jwtTokenExpire = jwtSettings.AccessTokenExpireInMinutes;

#if DEBUG
        var expiredAt = _dateTimeHelper.UtcNow.AddHours(12);
#else
        var expiredAt = _dateTimeHelper.UtcNow.AddMinutes(jwtTokenExpire);
#endif
        var encodedKey = Encoding.UTF8.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = expiredAt,
            Audience = jwtAudience,
            Issuer = jwtIssuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256Signature),
        };

        return TokenHandler.WriteToken(TokenHandler.CreateToken(tokenDescriptor));
    }
}

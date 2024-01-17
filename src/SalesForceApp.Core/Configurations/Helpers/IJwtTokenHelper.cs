using System.Security.Claims;

using SalesForceApp.Core.Configurations.Settings;

namespace SalesForceApp.Core.Configurations.Helpers;

public interface IJwtTokenHelper
{
    JwtSettings JwtSettings { get; }
    string GenerateNewToken(IEnumerable<Claim> claims);
    Ulid GenerateNewRefreshToken();
}

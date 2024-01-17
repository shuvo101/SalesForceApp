using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SalesForceApp.Api.Configurations.ServiceConfigurations;

public static class AuthConfiguration
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtKey = configuration.GetValue<string>("JWT:Key") ?? throw new Exception("JWT:Key is not found in appsettings");
            var jwtIssuer = configuration.GetValue<string>("JWT:Issuer") ?? throw new Exception("JWT:Issuer is not found in appsettings");
            var jwtAudience = configuration.GetValue<string>("JWT:Audience") ?? throw new Exception("JWT:Audience is not found in appsettings");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromSeconds(5),

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            };
        });

        services.AddAuthorization();

        return services;
    }
}

using System.Globalization;

using SalesForceApp.Core.Configurations.Helpers;

namespace SalesForceApp.Api.Configurations.Helpers;

public static class GetCurrentUserInfo
{
    public static (bool Success, long UserId) GetCurrentUserId(this HttpContext context)
    {
        var currentUserId = context.User.FindFirst(CustomClaimTypes.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return (false, default);
        }

        if (!long.TryParse(currentUserId, NumberStyles.None, provider: null, out var userId) || userId == default)
        {
            return (false, default);
        }

        return (true, userId);
    }
}

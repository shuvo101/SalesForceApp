using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using SalesForceApp.Api.Configurations.MinimalApi;
using SalesForceApp.Api.Configurations.Response;

using SalesForceApp.Core.Users.Model;
using SalesForceApp.Core.Users.Service;

using Starter.SalesForceApp.Api.Configs.Response;

namespace SalesForceApp.Api.Controllers.Users;

public class UserApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/account");

        group.MapPost("login", LoginAsync)
            .WithOpenApi()
            .WithSummary("Login user")
            .WithDescription("Login user with JWT token");
    }

    private async Task<Results<Ok<ApiResponse<UserLoginResponseModel>>, JsonHttpResult<ApiResponse<UserLoginResponseModel>>>> LoginAsync(
        [FromBody] UserLoginRequestModel request,
        [FromServices] UserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.LoginAsync(request, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<UserLoginResponseModel>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }
}

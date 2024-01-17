using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using SalesForceApp.Api.Configurations.MinimalApi;
using SalesForceApp.Api.Configurations.Response;

using SalesForceApp.Core.Tests.Entity;
using SalesForceApp.Core.Tests.Service;

using Starter.SalesForceApp.Api.Configs.Response;

namespace SalesForceApp.Api.Controllers.Tests;

public class TestApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/test").WithTags("Test");

        group.MapGet("get-application-user", GetApplication);
        group.MapGet("get-all-products", GetAllProductAsync);
        group.MapGet("get-vendors", GetVendors);
        group.MapGet("create-market-visit", CreateMarketVisit);
        group.MapGet("get-posts", GetPostsAsync)
            .WithOpenApi()
            .WithSummary("All Posts")
            .WithDescription("Get all posts");
    }

    private static async Task<Results<Ok<ApiResponse<ApplicationUser>>, JsonHttpResult<ApiResponse<ApplicationUser>>>> GetApplication([FromServices] TestService userService, CancellationToken cancellationToken)
    {
        var result = await userService.GetApplicationUserAsync(cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<ApplicationUser>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }

    private async Task<Results<Ok<ApiResponse<IEnumerable<Product>>>, JsonHttpResult<ApiResponse<IEnumerable<Product>>>>> GetAllProductAsync([FromServices] TestService testService, CancellationToken cancellationToken)
    {
        var result = await testService.GetAllProductAsync(cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<IEnumerable<Product>>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }

    private async Task<Results<Ok<ApiResponse<IEnumerable<Vendor>>>, JsonHttpResult<ApiResponse<IEnumerable<Vendor>>>>> GetVendors([FromServices] TestService testService, CancellationToken cancellationToken)
    {
        var result = await testService.GetVendors(cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<IEnumerable<Vendor>>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }

    private async Task<Results<Ok<ApiResponse<bool>>, JsonHttpResult<ApiResponse<bool>>>> CreateMarketVisit([FromServices] TestService testService, CancellationToken cancellationToken)
    {
        var result = await testService.CreateMarketVisit(cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<bool>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }

    private async Task<Results<Ok<ApiResponse<IEnumerable<Post>>>, JsonHttpResult<ApiResponse<IEnumerable<Post>>>>> GetPostsAsync([FromServices] TestService testService, CancellationToken cancellationToken)
    {
        var result = await testService.GetPostsAsync(cancellationToken).ConfigureAwait(false);
        if (result.IsError)
        {
            return ApiResponseResult.Problem<IEnumerable<Post>>(result.Error);
        }

        return ApiResponseResult.Success(result.Value);
    }
}

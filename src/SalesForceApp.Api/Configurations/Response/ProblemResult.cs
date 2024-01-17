using SalesForceApp.Api.Configurations.Helpers;

namespace Starter.SalesForceApp.Api.Configs.Response;

public class ProblemResult<T> : IResult
{
    private readonly ApiResponse<T> _response;

    public ProblemResult(ApiResponse<T> response)
    {
        _response = response;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var response = httpContext.Response;

        response.ContentType = HttpContentTypeConstants.ApplicationJson;
        response.StatusCode = _response.Status;

        await response.WriteAsJsonAsync(_response, cancellationToken: httpContext.RequestAborted).ConfigureAwait(false);
    }
}

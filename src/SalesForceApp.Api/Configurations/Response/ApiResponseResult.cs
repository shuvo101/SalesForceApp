using Lib.ErrorOr;

using Microsoft.AspNetCore.Http.HttpResults;

using Starter.SalesForceApp.Api.Configs.Response;

namespace SalesForceApp.Api.Configurations.Response;

public class ApiResponseResult
{
    public static Ok<ApiResponse<T>> Success<T>(T? data = default, string title = "Success")
    {
        return TypedResults.Ok(new ApiResponse<T>()
        {
            Message = title,
            Status = StatusCodes.Status200OK,
            Data = data,
        });
    }

    public static NotFound<ApiResponse<T>> NotFound<T>(string title = "Not found", T? data = default)
    {
        return TypedResults.NotFound(new ApiResponse<T>()
        {
            Message = title,
            Status = StatusCodes.Status404NotFound,
            Data = data,
        });
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
        string title = "Error occurred",
        int statusCode = StatusCodes.Status400BadRequest,
        T? data = default)
    {
        return TypedResults.Json(new ApiResponse<T>()
        {
            Message = title,
            Status = statusCode,
            Data = data,
        }, statusCode: statusCode);
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
    Error error,
    T? data = default)
    {
        var message = error.Description;
        var statusCode = error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Json(new ApiResponse<T>()
        {
            Message = message,
            Status = statusCode,
            Data = data,
        }, statusCode: statusCode);
    }

    public static BadRequest<ApiResponse<T>> ValidationProblem<T>(
        IDictionary<string, string[]> errors,
        string title = "Validation error occurred",
        T? data = default)
    {
        return TypedResults.BadRequest(new ApiResponse<T>()
        {
            Message = title,
            Status = StatusCodes.Status400BadRequest,
            Data = data,
            Errors = errors,
        });
    }
}

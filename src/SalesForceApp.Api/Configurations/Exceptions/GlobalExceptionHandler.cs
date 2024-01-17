using System.Data;
using System.Data.Common;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;

using Starter.SalesForceApp.Api.Configs.Response;

namespace SalesForceApp.Api.Configurations.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandler(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var isDevelopment = _env.IsDevelopment();

        var (statusCode, errorMessage) = exception switch
        {
            // Common
            FormatException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unable to format data"),
            InvalidCastException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unable to cast from one type to another type"),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Feature is not implemented"),
            NotSupportedException => (StatusCodes.Status500InternalServerError, "Feature is not supported"),
            ArgumentException e => (StatusCodes.Status501NotImplemented, isDevelopment ? e.Message : "Invalid argument is passed"),
            TimeoutException => (StatusCodes.Status504GatewayTimeout, "Operation timed out"),
            OperationCanceledException => (StatusCodes.Status500InternalServerError, "Operation cancelled"),

            // DB Related
            DBConcurrencyException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Failed to save, item is already changed by other user"),
            DbException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Database error occurred"),

            // JSON
            JsonException => (StatusCodes.Status400BadRequest, "Invalid JSON format"),

            // Memory
            InsufficientMemoryException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Insufficient memory, please contact system administrator"),

            // IO Related
            UnauthorizedAccessException => (StatusCodes.Status500InternalServerError, "Does not have enough permission to access file/folder"),
            PathTooLongException => (StatusCodes.Status500InternalServerError, "Path is too long"),
            DirectoryNotFoundException => (StatusCodes.Status500InternalServerError, "Directory is not found"),
            IOException => (StatusCodes.Status500InternalServerError, "Unknown I/O exception occurred"),

            Exception e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unknown error has occurred"),
            _ => (StatusCodes.Status500InternalServerError, "Unknown error has occurred")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(ApiResponse.Create(errorMessage, statusCode), cancellationToken: cancellationToken).ConfigureAwait(false);

        return true;
    }
}

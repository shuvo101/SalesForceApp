using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Diagnostics;

using Starter.SalesForceApp.Api.Configs.Response;

namespace SalesForceApp.Api.Configurations.Exceptions;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException fluentValidationException)
        {
            return false;
        }

        var dictionaryErrors = ToDictionaryErrors(fluentValidationException.Errors);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(ApiResponse.ValidationProblem(dictionaryErrors), cancellationToken: cancellationToken).ConfigureAwait(false);

        return true;
    }

    private static Dictionary<string, string[]> ToDictionaryErrors(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (var error in validationFailures.DistinctBy(x => x.PropertyName))
        {
            errors.Add(error.PropertyName, [error.ErrorMessage]);
        }

        return errors;
    }
}

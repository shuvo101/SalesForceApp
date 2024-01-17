namespace Lib.ErrorOr;

public readonly record struct Error
{
    public string? Title { get; }
    public string Description { get; }
    public string? ErrorProperty { get; }
    public string? ErrorCode { get; }
    public ErrorType Type { get; }

    private Error(
        string description,
        ErrorType type,
        string? title = null,
        string? errorProperty = null,
        string? errorCode = null)
    {
        Title = title;
        Description = description;
        Type = type;
        ErrorProperty = errorProperty;
        ErrorCode = errorCode;
    }

    public static Error Failure(
        string description = "A failure error has occurred.",
        string title = "Failure")
        => new(description, ErrorType.Failure, title);
    public static Error Unexpected(
        string description = "An unexpected error has occurred.",
        string title = "Unexpected error")
        => new(description, ErrorType.Unexpected, title);
    public static Error Validation(
        string description = "A validation error has occurred.",
        string title = "Validation error")
        => new(description, ErrorType.Validation, title);
    public static Error NotFound(
        string description = "A 'Not Found' error has occurred.",
        string title = "Not found")
        => new(description, ErrorType.NotFound, title);
    public static Error Unauthorized(
        string description = "You need to login first",
        string title = "Unauthorized")
        => new(description, ErrorType.Unauthorized, title);
    public static Error Forbidden(
        string description = "You are not allowed to perform this action",
        string title = "Forbidden")
        => new(description, ErrorType.Forbidden, title);
    public static Error Custom(
        ErrorType type,
        string description,
        string title)
        => new(description, type, title);
    public static Error PropertyError(
        string propertyName,
        string errorMessage,
        string? errorCode = null)
        => new(errorMessage, ErrorType.Validation, errorProperty: propertyName, errorCode: errorCode);
}

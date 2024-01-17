namespace Lib.ErrorOr;

public readonly record struct Success;

public class SuccessType
{
    public static Success Success => default;
    public static SuccessDetails<T> SuccessDetails<T>(string? title = "Success", T? data = default) => new(title, data);
}

public readonly record struct SuccessDetails<T>(
    string? Message,
    T? Data);

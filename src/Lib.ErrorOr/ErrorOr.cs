namespace Lib.ErrorOr;

public readonly record struct ErrorOr<TValue>
{
    private readonly TValue? _value = default;
    private readonly Error _error = Error.Unexpected(description: "Error cannot be retrieved from a successful status");

    public bool IsError { get; }
    public readonly Error Error => _error;
    public readonly TValue Value => _value!;

    public static ErrorOr<TValue> From(Error error)
    {
        return error;
    }

    // Constructors
    private ErrorOr(Error error)
    {
        _error = error;
        IsError = true;
    }

    private ErrorOr(TValue value)
    {
        _value = value;
        IsError = false;
    }

    // Implicit operators
    public static implicit operator ErrorOr<TValue>(TValue value)
    {
        return new ErrorOr<TValue>(value);
    }

    public static implicit operator ErrorOr<TValue>(Error error)
    {
        return new ErrorOr<TValue>(error);
    }

    // Actions
    public void Switch(Action<TValue> onValue, Action<Error> onError)
    {
        if (IsError)
        {
            onError(Error);
            return;
        }

        onValue(Value);
    }

    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<Error, Task> onError)
    {
        if (IsError)
        {
            await onError(Error).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    public TResult Match<TResult>(Func<TValue, TResult> onValue, Func<Error, TResult> onError)
    {
        if (IsError)
        {
            return onError(Error);
        }

        return onValue(Value);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<Error, Task<TResult>> onError)
    {
        if (IsError)
        {
            return await onError(Error).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}

public static class ErrorOr
{
    public static ErrorOr<TValue> From<TValue>(TValue value)
    {
        return value;
    }
}

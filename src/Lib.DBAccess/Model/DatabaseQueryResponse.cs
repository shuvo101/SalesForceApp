namespace Lib.DBAccess.Model;

public class DatabaseQueryResponse<T> where T : class, new()
{
    public bool Success { get; internal set; }
    public int ErrorCode { get; internal set; }
    public string? ErrorMessage { get; internal set; }
    public T Data { get; set; } = default!;
}

public class DatabaseQueryResponse
{
    public static DatabaseQueryResponse<Q> Failure<Q>(string errorMessage) where Q : class, new()
    {
        return new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            Data = default!,
        };
    }
}

namespace Lib.DBAccess.Model;

public class DatabaseNonQueryResponse
{
    public bool Success { get; internal set; }
    public int ErrorCode { get; internal set; }
    public string? ErrorMessage { get; internal set; }

    public static DatabaseNonQueryResponse Failure(string errorMessage)
    {
        return new()
        {
            Success = false,
            ErrorMessage = errorMessage,
        };
    }
}

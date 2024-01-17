namespace Lib.DBAccess.Model;

public class DatabaseNonQueryResponseWithOutputStatus : DatabaseNonQueryResponse
{
    public int OutputStatus { get; set; }

    public static new DatabaseNonQueryResponseWithOutputStatus Failure(string errorMessage)
    {
        return new()
        {
            Success = false,
            ErrorMessage = errorMessage,
        };
    }
}

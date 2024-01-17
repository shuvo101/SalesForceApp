namespace SalesForceApp.Core.Configurations.CommonModel;

public record struct FileSaveResult
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? SaveUrl { get; private set; }
    public string? FileName { get; private set; }
    public string? ThumbnailUrl { get; private set; }

    public static FileSaveResult Successful(string saveUrl, string fileName, string? thumbnailUrl = null)
    {
        return new FileSaveResult { Success = true, SaveUrl = saveUrl, FileName = fileName, ThumbnailUrl = thumbnailUrl };
    }

    public static FileSaveResult Failure(string errorMessage)
    {
        return new FileSaveResult { Success = false, ErrorMessage = errorMessage };
    }
}

namespace SalesForceApp.Api.Configurations.Settings;

public class FileUploadSettings
{
    public const string SectionName = "FileUpload";

    public string FileUploadFolderPath { get; set; } = string.Empty;
}

using System.Runtime.InteropServices;

using SalesForceApp.Api.Configurations.Enums;

namespace SalesForceApp.Api.Configurations.Helpers;

[StructLayout(LayoutKind.Auto)]
public record struct FileHelperOptions
{
    public FileUploadFolder FileUploadFolder { get; set; }
    public FilePrivacy FilePrivacy { get; set; }
    public bool GenerateThumbnail { get; set; }
    public int ImageThumbnailWidth { get; set; } = 400;
    public int MinimumAllowedImageSizeAfterThumbnailWillBeCreated { get; set; } = 512;

    public FileHelperOptions()
    {
    }
}

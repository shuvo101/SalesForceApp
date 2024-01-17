using SalesForceApp.Api.Configurations.Enums;
using SalesForceApp.Api.Configurations.Settings;

using SalesForceApp.Core.Configurations.CommonModel;
using SalesForceApp.Core.Configurations.Helpers;

using ImageMagick;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;

namespace SalesForceApp.Api.Configurations.Helpers;

public class FileHelper
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly FileUploadSettings _fileUploadOptions;

    public const string PublicFolder = "uploads/public";
    public const string PrivateFolder = "uploads/protected";

    public FileHelper(IWebHostEnvironment webHostEnvironment, IDateTimeHelper dateTimeHelper, IOptions<FileUploadSettings> fileUploadOptions)
    {
        _webHostEnvironment = webHostEnvironment;
        _dateTimeHelper = dateTimeHelper;
        _fileUploadOptions = fileUploadOptions.Value;
    }

    public string GetRootPath()
    {
        var fileUploadFolderPath = _fileUploadOptions.FileUploadFolderPath;
        var rootPath = string.IsNullOrEmpty(fileUploadFolderPath)
            ? Path.Combine(_webHostEnvironment.ContentRootPath, "..")
            : fileUploadFolderPath;

        return Path.GetFullPath(rootPath);
    }

    public string GetGeneratedFileName(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var currentDateTimeString = _dateTimeHelper.Now.ToString("yyyyMMddHHmmssfff", provider: null);
        return $"{currentDateTimeString}_{Guid.NewGuid()}{fileExtension}";
    }

    public async Task<FileSaveResult> SaveFile(
        IFormFile file,
        FileHelperOptions fileHelperOptions,
        CancellationToken cancellationToken = default)
    {
        if (file is null)
        {
            return FileSaveResult.Failure("File is empty");
        }

        var rootPath = GetRootPath();
        var folderPrivacyPath = fileHelperOptions.FilePrivacy == FilePrivacy.Private ? PrivateFolder : PublicFolder;
        var fileUploadFolderName = fileHelperOptions.FileUploadFolder.GetDisplayName();

        var finalFileSaveDirectory = Path.Combine(rootPath, folderPrivacyPath, fileUploadFolderName);
        Directory.CreateDirectory(finalFileSaveDirectory);

        var generatedFileName = GetGeneratedFileName(file);
        var finalFileDiskPath = Path.Combine(finalFileSaveDirectory, generatedFileName);

        var fileStream = new FileStream(finalFileDiskPath, FileMode.Create, FileAccess.Write);
        await using (fileStream.ConfigureAwait(false))
        {
            await file.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }

        var fileUrl = $"/{folderPrivacyPath}/{fileUploadFolderName}/{generatedFileName}";

        var imageThumbnailWidth = fileHelperOptions.ImageThumbnailWidth;
        var minimumAllowedImageSizeAfterThumbnailWillBeCreated = fileHelperOptions.MinimumAllowedImageSizeAfterThumbnailWillBeCreated;

        if (fileHelperOptions.GenerateThumbnail && file.Length > minimumAllowedImageSizeAfterThumbnailWillBeCreated)
        {
            var fileReadStream = file.OpenReadStream();
            await using (fileReadStream.ConfigureAwait(false))
            {
                using var image = new MagickImage(fileReadStream);

                if (imageThumbnailWidth > image.Width)
                {
                    imageThumbnailWidth = image.Width;
                }

                var ratio = (double)image.Width / image.Height;
                int targetHeight = (int)((double)image.Height / image.Width * imageThumbnailWidth);
                var size = new MagickGeometry(imageThumbnailWidth, targetHeight)
                {
                    IgnoreAspectRatio = true,
                };
                image.Resize(size);

                var thumbnailDiskFileName = $"thumbnail_{generatedFileName}";
                var thumbnailFileUrl = $"/{folderPrivacyPath}/{fileUploadFolderName}/{thumbnailDiskFileName}";
                await image.WriteAsync(Path.Combine(finalFileSaveDirectory, thumbnailDiskFileName), cancellationToken).ConfigureAwait(false);

                return FileSaveResult.Successful(fileUrl, file.FileName, thumbnailFileUrl);
            }
        }

        return FileSaveResult.Successful(fileUrl, file.FileName);
    }
}

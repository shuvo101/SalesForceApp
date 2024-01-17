using System.Net.Mime;
using System.Web;

using SalesForceApp.Api.Configurations.Helpers;
using SalesForceApp.Api.Configurations.MinimalApi;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc;

namespace SalesForceApp.Api.Controllers.Files;

public class FileApi : IEndpoint
{
    private const string ContentDisposition = "Content-Disposition";
    private const string OctetStream = "application/octet-stream";
    private static readonly FileExtensionContentTypeProvider Provider = new();

    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/file");

        group.MapGet("/{url}", FileHandle);
    }

    private IResult FileHandle(
        [FromRoute] string url,
        [FromQuery] bool download,
        [FromQuery] string? name,
        [FromServices] FileHelper fileHelper,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {

        url = HttpUtility.UrlDecode(url);
        name = string.IsNullOrEmpty(name) ? null : HttpUtility.UrlDecode(name);

        var isPublicFolder = url.StartsWith($"/{FileHelper.PublicFolder}", StringComparison.OrdinalIgnoreCase);
        var isPrivateFolder = url.StartsWith($"/{FileHelper.PrivateFolder}", StringComparison.OrdinalIgnoreCase);

        if (!isPublicFolder && !isPrivateFolder)
        {
            return Results.NotFound();
        }

        if (isPrivateFolder && !httpContext.GetCurrentUserId().Success)
        {
            return Results.Unauthorized();
        }

        var rootPath = fileHelper.GetRootPath();
        var fileDiskPath = Path.GetFullPath(Path.Combine(rootPath, url.TrimStart('/')));
        if (!fileDiskPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) || !File.Exists(fileDiskPath))
        {
            return Results.NotFound();
        }

        var extension = Path.GetExtension(fileDiskPath);
        var filename = Path.GetFileName(fileDiskPath);

        Provider.TryGetContentType(extension, out string? contentType);

        contentType = download
            ? OctetStream
            : string.IsNullOrEmpty(contentType) ? OctetStream : contentType;

        var contentDisposition = new ContentDisposition
        {
            FileName = name ?? filename,
            Inline = !download,
        };
        httpContext.Response.Headers.Append(ContentDisposition, contentDisposition.ToString());

        var fileStream = File.OpenRead(fileDiskPath);
        return Results.File(fileStream, contentType);
    }
}

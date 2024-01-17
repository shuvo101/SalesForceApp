using System.Text;

using SalesForceApp.Core.Configurations.Helpers;

using Serilog;

namespace SalesForceApp.Api.Configurations.Middleware;

public class LogRequestResponseMiddleware : IMiddleware
{
    private readonly IDateTimeHelper _dateTimeHelper;

    public LogRequestResponseMiddleware(IDateTimeHelper dateTimeHelper)
    {
        _dateTimeHelper = dateTimeHelper;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestStartTime = _dateTimeHelper.Now;
        var traceId = context.TraceIdentifier;
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var requestMethod = context.Request.Method;
        var requestBodyText = await ReadRequestBody(context.Request).ConfigureAwait(false);

        var responseBodyText = string.Empty;
        var responseStatus = StatusCodes.Status500InternalServerError;
        var isResponseOk = false;

        try
        {
            var originalResponseBody = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await next(context).ConfigureAwait(false);

            responseBodyText = await ReadResponseBody(context, newResponseBody).ConfigureAwait(false);
            isResponseOk = context.Response.StatusCode == StatusCodes.Status200OK;
            responseStatus = context.Response.StatusCode;

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody, context.RequestAborted).ConfigureAwait(false);
        }
        finally
        {
            var requestEndTime = _dateTimeHelper.Now;

            var obj = new RequestResponseLog(
                requestStartTime: requestStartTime,
                requestEndTime: requestEndTime,
                traceId: traceId,
                clientIp: clientIp,
                requestMethod: requestMethod,
                requestBody: requestBodyText,
                responseBody: responseBodyText,
                responseStatus: responseStatus);

            Log.Information(
                "Request time: {Request}, Time taken: {TimeTaken}(ms), Method: {Method}, Client IP: {ClientIp}, Response Status: {Status}",
                obj.RequestStartTime,
                obj.RequestTimeTaken.Microseconds,
                obj.RequestMethod,
                obj.ClientIp,
                obj.ResponseStatus);
        }
    }

    private static async Task<string> ReadResponseBody(HttpContext context, MemoryStream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);

        return await new StreamReader(context.Response.Body).ReadToEndAsync(context.RequestAborted).ConfigureAwait(false);
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();

        using var memoryStream = new MemoryStream();

        await request.Body.CopyToAsync(memoryStream, request.HttpContext.RequestAborted).ConfigureAwait(false);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        return await reader.ReadToEndAsync(request.HttpContext.RequestAborted).ConfigureAwait(false);
    }

    public record struct RequestResponseLog
    {
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestEndTime { get; set; }
        public readonly TimeSpan RequestTimeTaken => RequestEndTime - RequestStartTime;
        public string? TraceId { get; set; }
        public string ClientIp { get; set; }
        public string RequestMethod { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int ResponseStatus { get; set; }

        public RequestResponseLog(
            DateTime requestStartTime,
            DateTime requestEndTime,
            string? traceId,
            string clientIp,
            string requestMethod,
            string requestBody,
            string responseBody,
            int responseStatus)
        {
            RequestStartTime = requestStartTime;
            RequestEndTime = requestEndTime;
            TraceId = traceId;
            ClientIp = clientIp;
            RequestMethod = requestMethod;
            RequestBody = requestBody;
            ResponseBody = responseBody;
            ResponseStatus = responseStatus;
        }
    }
}

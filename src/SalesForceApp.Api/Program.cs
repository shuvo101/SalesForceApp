using SalesForceApp.Api.Configurations.Middleware;
using SalesForceApp.Api.Configurations.MinimalApi;
using SalesForceApp.Api.Configurations.ServiceConfigurations;
using SalesForceApp.Api.Configurations.Settings;

using Microsoft.OpenApi.Models;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
var logConfigSettings = LogConfigSettings.Get(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    if (logConfigSettings.FileLogConfig is not null)
    {
        var fileLogConfig = logConfigSettings.FileLogConfig;

        configuration.WriteTo.Async(x => x.File(
            fileLogConfig.Path,
            restrictedToMinimumLevel: fileLogConfig.LogLevel,
            rollingInterval: fileLogConfig.RollingInterval,
            retainedFileCountLimit: fileLogConfig.RetainedFileCountLimit,
            flushToDiskInterval: TimeSpan.FromSeconds(fileLogConfig.FlushToDiskIntervalInSeconds),
            shared: fileLogConfig.Shared,
            buffered: fileLogConfig.Buffered));
    }

    configuration.ReadFrom.Configuration(context.Configuration);
});
#endregion

builder.Services.AddApiConfigurations(builder.Configuration);

var apiConfig = ApiConfigSettings.Get(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(opt => { });

#region Swagger
app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, _) =>
{
    var servers = new List<OpenApiServer>();

    foreach (var swaggerBaseUrl in apiConfig.SwaggerBaseUrlList)
    {
        servers.Add(new OpenApiServer
        {
            Url = $"{swaggerBaseUrl.Scheme}://{swaggerBaseUrl.Host}/{swaggerBaseUrl.Prefix}",
        });
    }

    swaggerDoc.Servers = servers;
}));
app.UseSwaggerUI();
#endregion

if (apiConfig.UseHttps)
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

if (logConfigSettings.LogRequestResponse)
{
    app.UseMiddleware<LogRequestResponseMiddleware>();
}

foreach (var endpoint in app.Services.GetRequiredService<IEnumerable<IEndpoint>>())
{
    endpoint.MapRoutes(app);
}

await app.RunAsync().ConfigureAwait(false);


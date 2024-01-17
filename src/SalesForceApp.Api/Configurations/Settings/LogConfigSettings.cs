using Serilog;
using Serilog.Events;

namespace SalesForceApp.Api.Configurations.Settings;

public class LogConfigSettings
{
    public const string SectionName = "LogConfig";
    public FileLog? FileLogConfig { get; set; }
    public bool LogRequestResponse { get; set; }

    public static LogConfigSettings Get(IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);

        return section.Get<LogConfigSettings>() ?? throw new Exception($"Could not load section: {SectionName} from appsettings");
    }

    public class FileLog
    {
        public LogEventLevel LogLevel { get; set; }
        public string Path { get; set; } = string.Empty;
        public RollingInterval RollingInterval { get; set; }
        public int? RetainedFileCountLimit { get; set; }
        public int FlushToDiskIntervalInSeconds { get; set; }
        public bool Buffered { get; set; } = true;
        public bool Shared { get; set; }
    }
}

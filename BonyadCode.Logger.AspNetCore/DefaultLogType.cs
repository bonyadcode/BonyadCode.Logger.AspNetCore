using Serilog;
using Serilog.Events;

namespace BonyadCode.Logger.AspNetCore;

public class DefaultLogType : ILogType
{
    public string Name { get; init; } = "Default";
    public string? Folder { get; init; } = "default";

    public LogEventLevel LogEventLevel { get; init; } = LogEventLevel.Verbose;
    public RollingInterval RollingInterval { get; init; } = RollingInterval.Hour;
    public string GetLogPath(string? logId) => 
        Path.Combine("app-logs", Folder!, logId is null 
            ? $"log_{Name.ToLowerInvariant()}_.md" 
            : $"log_{Name.ToLowerInvariant()}_{logId}_.md");
    public LogEventLevel GetLogEventLevel() => LogEventLevel;

    public RollingInterval GetRollingInterval() => RollingInterval;

    public string GetOutputTemplate() =>
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}

using Serilog;
using Serilog.Events;

namespace BonyadCode.Logger.AspNetCore;

/// <summary>
/// Default implementation of <see cref="ILogType"/> with configurable
/// log folder, name, level, rolling interval, and output template.
/// </summary>
public class DefaultLogType : ILogType
{
    /// <summary>
    /// Logical name of the log type. Used as an identifier.
    /// </summary>
    public string Name { get; init; } = "Default";

    /// <summary>
    /// Folder name under "app-logs" where this log type will be stored.
    /// </summary>
    public string? Folder { get; init; } = "default";

    /// <summary>
    /// Minimum log level to write logs of this type.
    /// </summary>
    public LogEventLevel LogEventLevel { get; init; } = LogEventLevel.Verbose;

    /// <summary>
    /// Determines the rolling interval of the log file (e.g., Hour, Day, Minute).
    /// </summary>
    public RollingInterval RollingInterval { get; init; } = RollingInterval.Hour;

    /// <summary>
    /// Gets the full path for the log file based on optional log identifier.
    /// </summary>
    /// <param name="logId">Optional identifier to differentiate logs by instance.</param>
    /// <returns>File path including log name and identifier.</returns>
    public string GetLogPath(string? logId) =>
        Path.Combine("app-logs", Folder!, logId is null
            ? $"log_{Name.ToLowerInvariant()}_.md"
            : $"log_{Name.ToLowerInvariant()}_{logId}_.md");

    /// <summary>
    /// Gets the log level threshold used for writing logs.
    /// </summary>
    public LogEventLevel GetLogEventLevel() => LogEventLevel;

    /// <summary>
    /// Gets the rolling interval used to rotate the log file.
    /// </summary>
    public RollingInterval GetRollingInterval() => RollingInterval;

    /// <summary>
    /// Gets the template used to format the output log entries.
    /// </summary>
    public string GetOutputTemplate() =>
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}

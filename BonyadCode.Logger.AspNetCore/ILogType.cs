using Serilog;
using Serilog.Events;

namespace BonyadCode.Logger.AspNetCore;

/// <summary>
/// Interface defining a structured log type for use with the logger.
/// Provides log metadata such as name, storage path, log level, and format.
/// </summary>
public interface ILogType
{
    /// <summary>
    /// The name identifier of the log type.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the full path to the log file, optionally scoped by a unique log identifier.
    /// </summary>
    /// <param name="logId">An optional identifier to create per-request or per-operation log files.</param>
    /// <returns>File path for the log.</returns>
    string GetLogPath(string? logId);

    /// <summary>
    /// Specifies the minimum severity level required for a log to be written.
    /// </summary>
    /// <returns>Minimum log level for writing to this log type.</returns>
    LogEventLevel GetLogEventLevel();

    /// <summary>
    /// Specifies how often the log file should be rolled (e.g., daily, hourly).
    /// </summary>
    /// <returns>Rolling interval for the log file.</returns>
    RollingInterval GetRollingInterval();

    /// <summary>
    /// Returns the output format template used when writing log entries.
    /// </summary>
    /// <returns>Log message formatting template.</returns>
    string GetOutputTemplate();
}
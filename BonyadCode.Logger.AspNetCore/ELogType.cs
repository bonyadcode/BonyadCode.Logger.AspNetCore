namespace BonyadCode.Logger.AspNetCore;

/// <summary>
/// Enum representing predefined log types with semantic categories for structured logging.
/// These types are mapped to <see cref="ILogType"/> instances via <see cref="PredefinedLogTypes"/>.
/// </summary>
public enum ELogType : byte
{
    /// <summary>
    /// Default general-purpose log type.
    /// </summary>
    Default,

    /// <summary>
    /// Logs related to application startup events.
    /// </summary>
    Startup,

    /// <summary>
    /// Logs startup-related exceptions.
    /// </summary>
    StartupException,

    /// <summary>
    /// General tracing logs for business or infrastructure events.
    /// </summary>
    TraceLog,

    /// <summary>
    /// Exceptions captured during trace operations.
    /// </summary>
    TraceLogException,

    /// <summary>
    /// Critical failures encountered during tracing.
    /// </summary>
    TraceLogFailure,

    /// <summary>
    /// General exception logging.
    /// </summary>
    Exception,

    /// <summary>
    /// Database-specific exceptions.
    /// </summary>
    ExceptionDatabase,

    /// <summary>
    /// Logs indicating data tampering or integrity issues.
    /// </summary>
    ExceptionDataTamper,

    /// <summary>
    /// Failures during exception handling or recovery operations.
    /// </summary>
    ExceptionFailure,

    /// <summary>
    /// Non-exception failures such as command execution or workflow failures.
    /// </summary>
    Failure,
}
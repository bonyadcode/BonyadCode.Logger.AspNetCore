using Serilog;
using Serilog.Events;

namespace BonyadCode.Logger.AspNetCore;

public static class PredefinedLogTypes
{
    private static readonly Dictionary<ELogType, ILogType> Map = new()
    {
        [ELogType.Default] = new DefaultLogType
        {
            Name = "Default", Folder = "default", LogEventLevel = LogEventLevel.Verbose,
            RollingInterval = RollingInterval.Hour
        },
        [ELogType.Startup] = new DefaultLogType
        {
            Name = "Startup", Folder = "startup", LogEventLevel = LogEventLevel.Information,
            RollingInterval = RollingInterval.Hour
        },
        [ELogType.StartupException] = new DefaultLogType
        {
            Name = "StartupException", Folder = "startup/exceptions", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.TraceLog] = new DefaultLogType
        {
            Name = "TraceLog", Folder = "tracelogs", LogEventLevel = LogEventLevel.Information,
            RollingInterval = RollingInterval.Hour
        },
        [ELogType.TraceLogException] = new DefaultLogType
        {
            Name = "TraceLogException", Folder = "tracelogs/exceptions", LogEventLevel = LogEventLevel.Error,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.TraceLogFailure] = new DefaultLogType
        {
            Name = "TraceLogFailure", Folder = "tracelogs/failures", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.Exception] = new DefaultLogType
        {
            Name = "Exception", Folder = "exceptions", LogEventLevel = LogEventLevel.Error,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.ExceptionDatabase] = new DefaultLogType
        {
            Name = "ExceptionDatabase", Folder = "exceptions/database", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.ExceptionDataTamper] = new DefaultLogType
        {
            Name = "ExceptionDataTamper", Folder = "exceptions/datatamper", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.ExceptionFailure] = new DefaultLogType
        {
            Name = "ExceptionFailure", Folder = "exceptions/failure", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
        [ELogType.Failure] = new DefaultLogType
        {
            Name = "Failure", Folder = "failures", LogEventLevel = LogEventLevel.Fatal,
            RollingInterval = RollingInterval.Minute
        },
    };

    public static ILogType Get(ELogType type) => Map.TryGetValue(type, out var logType)
        ? logType
        : Map[ELogType.Default];

    public static IEnumerable<KeyValuePair<string, ILogType>> GetAll()
        => Map.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
}
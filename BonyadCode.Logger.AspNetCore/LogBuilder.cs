using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace BonyadCode.Logger.AspNetCore;

public static class LogBuilder
{
    private static readonly ConcurrentDictionary<string, Lazy<Serilog.Core.Logger>> Loggers = new();

    public static Task LogAsync(ELogType logType, object? log)
        => LogAsync(PredefinedLogTypes.Get(logType), log);

    public static Task LogExceptionAsync(ELogType logType, Exception ex)
        => LogExceptionAsync(PredefinedLogTypes.Get(logType), ex);

    public static Task LogAsync(string logTypeName, object? log)
        => LogAsync(LoggerRegistry.Get(logTypeName), log);

    public static Task LogExceptionAsync(string logTypeName, Exception ex)
        => LogExceptionAsync(LoggerRegistry.Get(logTypeName), ex);

    public static async Task LogAsync(ILogType logType, object? log)
    {
        var utcNow = DateTime.UtcNow;
        log ??= new { Message = "No log data provided." };

        var logText = FormatLogText(utcNow, logType, log);
        var logger = GetLoggerInstance(logType);
        await Task.Run(() => logger.Write(logType.GetLogEventLevel(), logText));
    }

    public static async Task LogExceptionAsync(ILogType logType, Exception ex)
    {
        var dictionary = ConvertExceptionToDictionary(ex);
        await LogAsync(logType, JsonConvert.SerializeObject(dictionary, Formatting.Indented));
    }

    private static Serilog.Core.Logger GetLoggerInstance(ILogType logType)
    {
        var key = logType.Name.ToLowerInvariant();
        if (!Loggers.TryGetValue(key, out var value))
        {
            value = new Lazy<Serilog.Core.Logger>(() =>
                new LoggerConfiguration()
                    .WriteTo.File(
                        path: logType.GetLogPath(null),
                        levelSwitch: new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Verbose },
                        outputTemplate: logType.GetOutputTemplate(),
                        rollingInterval: logType.GetRollingInterval())
                    .CreateLogger()
            );
            Loggers[key] = value;
        }

        return value.Value;
    }

    private static string FormatLogText(DateTime utcNow, ILogType logType, object? content)
    {
        return $"""

                 -----
                 ----------
                 --------------- 
                 Start of {logType.Name} Log at utc: {utcNow:yyyy/MM/dd HH:mm:ss zzz}, local: {utcNow.ToLocalTime():yyyy/MM/dd HH:mm:ss zzz}
                 ----- 
                 {content}
                 ----- 
                 End of {logType.Name} Log at utc: {utcNow:yyyy/MM/dd HH:mm:ss zzz}, local: {utcNow.ToLocalTime():yyyy/MM/dd HH:mm:ss zzz}
                 ---------------
                 ----------
                 ----- 
                """;
    }

    private static Dictionary<string, string[]> ConvertExceptionToDictionary(Exception exception)
    {
        var dictionary = new Dictionary<string, string[]>();
        var properties = exception.GetType().GetProperties();
        foreach (var property in properties)
        {
            dictionary[property.Name] = property.Name != "InnerException"
                ? [property.GetValue(exception)?.ToString() ?? string.Empty]
                : exception.InnerException != null
                    ?
                    [
                        JsonConvert.SerializeObject(ConvertExceptionToDictionary(exception.InnerException),
                            Formatting.Indented)
                    ]
                    : [];
        }

        return dictionary;
    }
}
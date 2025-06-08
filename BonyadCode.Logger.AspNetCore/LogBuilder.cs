using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace BonyadCode.Logger.AspNetCore
{
    /// <summary>
    /// Central logging utility that handles writing structured logs using Serilog.
    /// Supports dynamic log type configurations and exception logging.
    /// </summary>
    public static class LogBuilder
    {
        /// <summary>
        /// Holds lazily initialized Serilog loggers for each unique log type.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<Serilog.Core.Logger>> Loggers = new();

        /// <summary>
        /// Logs a structured message asynchronously using a predefined log type.
        /// </summary>
        /// <param name="logType">Predefined log type enum.</param>
        /// <param name="log">The log content object, can be null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task LogAsync(ELogType logType, object? log)
            => LogAsync(PredefinedLogTypes.Get(logType), log);

        /// <summary>
        /// Logs an exception asynchronously using a predefined log type.
        /// </summary>
        /// <param name="logType">Predefined log type enum.</param>
        /// <param name="ex">The exception to log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task LogExceptionAsync(ELogType logType, Exception ex)
            => LogExceptionAsync(PredefinedLogTypes.Get(logType), ex);

        /// <summary>
        /// Logs a structured message asynchronously using a custom log type name.
        /// </summary>
        /// <param name="logTypeName">Name of the custom log type.</param>
        /// <param name="log">The log content object, can be null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task LogAsync(string logTypeName, object? log)
            => LogAsync(LoggerRegistry.Get(logTypeName), log);

        /// <summary>
        /// Logs an exception asynchronously using a custom log type name.
        /// </summary>
        /// <param name="logTypeName">Name of the custom log type.</param>
        /// <param name="ex">The exception to log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task LogExceptionAsync(string logTypeName, Exception ex)
            => LogExceptionAsync(LoggerRegistry.Get(logTypeName), ex);

        /// <summary>
        /// Logs a structured message asynchronously using the provided ILogType.
        /// </summary>
        /// <param name="logType">The log type interface instance.</param>
        /// <param name="log">The log content object, can be null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task LogAsync(ILogType logType, object? log)
        {
            var utcNow = DateTime.UtcNow;
            log ??= new { Message = "No log data provided." };

            var logText = FormatLogText(utcNow, logType, log);
            var logger = GetLoggerInstance(logType);
            await Task.Run(() => logger.Write(logType.GetLogEventLevel(), logText));
        }

        /// <summary>
        /// Logs a formatted exception asynchronously using the provided ILogType.
        /// Serializes the exception and inner exceptions into a structured JSON string.
        /// </summary>
        /// <param name="logType">The log type interface instance.</param>
        /// <param name="ex">The exception to log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task LogExceptionAsync(ILogType logType, Exception ex)
        {
            var dictionary = ConvertExceptionToDictionary(ex);
            var json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            await LogAsync(logType, json);
        }

        /// <summary>
        /// Lazily retrieves or creates a Serilog logger instance for the given log type.
        /// </summary>
        /// <param name="logType">The log type interface instance.</param>
        /// <returns>A Serilog logger configured for the log type.</returns>
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

        /// <summary>
        /// Formats log content with timestamps and section headers for clarity.
        /// </summary>
        /// <param name="utcNow">The current UTC time.</param>
        /// <param name="logType">The log type interface instance.</param>
        /// <param name="content">The content to log.</param>
        /// <returns>A formatted string ready for logging.</returns>
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

        /// <summary>
        /// Recursively converts an exception and its inner exceptions to a structured dictionary.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <returns>A dictionary representation of the exception and inner exceptions.</returns>
        private static Dictionary<string, string[]> ConvertExceptionToDictionary(Exception exception)
        {
            var dictionary = new Dictionary<string, string[]>();
            var properties = exception.GetType().GetProperties();
            foreach (var property in properties)
            {
                dictionary[property.Name] = property.Name != "InnerException"
                    ? [property.GetValue(exception)?.ToString() ?? string.Empty]
                    : exception.InnerException != null
                        ? new[]
                        {
                            JsonConvert.SerializeObject(ConvertExceptionToDictionary(exception.InnerException),
                                Formatting.Indented)
                        }
                        : Array.Empty<string>();
            }

            return dictionary;
        }
    }
}
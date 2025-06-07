using Serilog;
using Serilog.Events;

namespace BonyadCode.Logger.AspNetCore;

public interface ILogType
{
    string Name { get; }
    string GetLogPath(string? logId);
    LogEventLevel GetLogEventLevel();
    RollingInterval GetRollingInterval();
    string GetOutputTemplate();
}
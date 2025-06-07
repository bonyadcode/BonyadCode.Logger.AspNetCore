using System.Collections.Concurrent;

namespace BonyadCode.Logger.AspNetCore;

public static class LoggerRegistry
{
    private static readonly ConcurrentDictionary<string, ILogType> Registry = new();

    static LoggerRegistry()
    {
        // Register predefined enums by default
        foreach (var kvp in PredefinedLogTypes.GetAll())
            Register(kvp);
    }

    public static void Register(ILogType logType)
        => Registry[logType.Name.ToLowerInvariant()] = logType;

    public static void Register(KeyValuePair<string, ILogType> pair)
        => Registry[pair.Key.ToLowerInvariant()] = pair.Value;

    public static ILogType Get(string name)
    {
        if (!Registry.TryGetValue(name.ToLowerInvariant(), out var type))
            throw new KeyNotFoundException($"Log type '{name}' is not registered.");

        return type;
    }

    public static IEnumerable<ILogType> GetAll() => Registry.Values;
}

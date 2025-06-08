using System.Collections.Concurrent;

namespace BonyadCode.Logger.AspNetCore;

/// <summary>
/// Manages registration and retrieval of custom <see cref="ILogType"/> implementations.
/// Supports dynamic and predefined log type mapping by name.
/// </summary>
public static class LoggerRegistry
{
    /// <summary>
    /// Thread-safe dictionary holding all registered log types.
    /// </summary>
    private static readonly ConcurrentDictionary<string, ILogType> Registry = new();

    static LoggerRegistry()
    {
        // Register predefined enums by default
        foreach (var kvp in PredefinedLogTypes.GetAll())
            Register(kvp);
    }

    /// <summary>
    /// Registers a custom <see cref="ILogType"/> instance by its name.
    /// </summary>
    /// <param name="logType">The log type instance to register.</param>
    public static void Register(ILogType logType)
        => Registry[logType.Name.ToLowerInvariant()] = logType;

    /// <summary>
    /// Registers a key-value pair representing a log type.
    /// </summary>
    /// <param name="pair">The name and log type pair to register.</param>
    public static void Register(KeyValuePair<string, ILogType> pair)
        => Registry[pair.Key.ToLowerInvariant()] = pair.Value;

    /// <summary>
    /// Retrieves a registered <see cref="ILogType"/> by its name.
    /// </summary>
    /// <param name="name">The name of the log type.</param>
    /// <returns>The corresponding <see cref="ILogType"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the log type is not registered.</exception>
    public static ILogType Get(string name)
    {
        if (!Registry.TryGetValue(name.ToLowerInvariant(), out var type))
            throw new KeyNotFoundException($"Log type '{name}' is not registered.");

        return type;
    }

    /// <summary>
    /// Returns all registered log types.
    /// </summary>
    /// <returns>enumerable of all <see cref="ILogType"/> values.</returns>
    public static IEnumerable<ILogType> GetAll() => Registry.Values;
}

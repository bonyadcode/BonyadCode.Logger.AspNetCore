In the name of God

# BonyadCode.Logger.AspNetCore

A flexible, structured logging utility built on Serilog — designed for ASP.NET Core applications. Supports predefined
and custom log types with categorized log paths, output templates, and log levels. Ideal for capturing detailed
application traces, failures, exceptions, and custom audit trails.

---

## ✨ Features

* **Categorized Logging**: Logs organized by type, purpose, and file path.
* **Predefined Log Types**: Built-in enums for common app scenarios (e.g., Startup, Exceptions, Traces).
* **Custom Log Types**: Define your own log types with full control over paths, levels, and formatting.
* **Exception Logging**: Automatically capture and serialize exception details.
* **Simple Integration**: Log with a single method call — no need for boilerplate setup.

---

## 📦 Installation

```bash
dotnet add package BonyadCode.Logger.AspNetCore
```

---

## 🚀 Quick Examples

### ✅ Log a Startup Message (using built-in type)

```csharp
using BonyadCode.Logger.AspNetCore;

await LogBuilder.LogAsync(
    PredefinedLogTypes.Get(ELogType.Startup),
    new { Message = "Application has started." }
);
```

### ✅ Log an Exception (using built-in type)

```csharp
try
{
    // Risky code
}
catch (Exception ex)
{
    await LogBuilder.LogExceptionAsync(
        PredefinedLogTypes.Get(ELogType.Exception),
        ex
    );
}
```

### ✅ Log with a Custom Log Type

```csharp
public class AuditLogType : ILogType
{
    public string Name => "Audit";
    public string GetPath(string? logId) =>
        Path.Combine("custom-logs", "audit", $"audit_{logId}_.log");

    public LogEventLevel GetLogEventLevel() => LogEventLevel.Information;
    public RollingInterval GetRollingInterval() => RollingInterval.Day;

    public string GetOutputTemplate() =>
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [AUDIT] {Message:lj}{NewLine}{Exception}";
}

// Use it:
var audit = new AuditLogType();
await LogBuilder.LogAsync(audit, new { User = "admin", Action = "Deleted item #42" });
```

---

## 📘 Predefined Log Types

| Enum Value            | Folder Path                      | Level       | Rolling |
|-----------------------|----------------------------------|-------------|---------|
| `Default`             | `app-logs/default`               | Verbose     | Hour    |
| `Startup`             | `app-logs/startup`               | Information | Hour    |
| `StartupException`    | `app-logs/startup/exceptions`    | Fatal       | Minute  |
| `TraceLog`            | `app-logs/tracelogs`             | Information | Hour    |
| `TraceLogException`   | `app-logs/tracelogs/exceptions`  | Error       | Minute  |
| `TraceLogFailure`     | `app-logs/tracelogs/failures`    | Fatal       | Minute  |
| `Exception`           | `app-logs/exceptions`            | Error       | Minute  |
| `ExceptionDatabase`   | `app-logs/exceptions/database`   | Fatal       | Minute  |
| `ExceptionDataTamper` | `app-logs/exceptions/datatamper` | Fatal       | Minute  |
| `ExceptionFailure`    | `app-logs/exceptions/failure`    | Fatal       | Minute  |
| `Failure`             | `app-logs/failures`              | Fatal       | Minute  |

Retrieve via:

```csharp
var logType = PredefinedLogTypes.Get(ELogType.TraceLog);
```

---

## ⚙️ How It Works

* Logs are written using Serilog to file.
* Log types (via `ILogType`) define how/where logs are saved:

    * File path
    * Log level
    * Rolling interval
    * Output template
* Logs are formatted with timestamp boundaries and serialized objects or exceptions.

---

## 🧹 Extension Points

You can register your own log types simply by implementing the `ILogType` interface:

```csharp
public interface ILogType
{
    string Name { get; }
    string GetPath(string? logId);
    LogEventLevel GetLogEventLevel();
    RollingInterval GetRollingInterval();
    string GetOutputTemplate();
}
```

---

## 🧪 Optional Synchronous Usage

If you prefer fire-and-forget, the methods are safe to call without `await`:

```csharp
_ = LogBuilder.LogAsync(...);
```

You may also provide a synchronous wrapper in your project:

```csharp
public static void Log(ILogType type, object log) =>
    Task.Run(() => LogBuilder.LogAsync(type, log));
```

---

## 🤝 Contributing

Issues, ideas, and PRs are welcome!
Star the repo or submit improvements at [GitHub →](https://github.com/bonyadcode/BonyadCode.Logger.AspNetCore)

---

## 📄 License

Apache 2.0 — see the [LICENSE](LICENSE) file.

---

## 📦 Links

* [NuGet](https://www.nuget.org/packages/BonyadCode.Logger.AspNetCore)
* [GitHub](https://github.com/bonyadcode/BonyadCode.Logger.AspNetCore)

Usage Conditions:
- This program must not be used for any military or governmental purposes without the owner's consent.
- This program must not be used for any inhumane purposes.
- This program must not be used for any illegal or haram activities.
using NLog;

namespace Addams.Core.Logs;

public static class LoggerManager
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static event Action<LogEntry>? OnLog;

    /// <summary>
    /// Log into file and guit
    /// </summary>
    public static void Log(string message, Level level = Level.Info)
    {
        var entry = new LogEntry(message, level, DateTime.UtcNow);

        Logger.Log(entry.NLevel, message);

        OnLog?.Invoke(entry);
    }
}

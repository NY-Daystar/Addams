using NLog;
using System.Drawing;
using System.Globalization;

namespace Addams.Core.Logs;

public class LogEntry(string message, Level level, DateTime Timestamp)
{
    public string Message { get; set; } = message;

    public Level Level { get; set; } = level;

    public Color Color { get; set; } = level switch
    {
        Level.Debug => Color.Blue,
        Level.Info => Color.Black,
        Level.Warning => Color.Orange,
        Level.Error => Color.Red,
        Level.Ok => Color.Green,
        Level.Important => Color.Magenta,
        _ => Color.Black
    };

    public LogLevel NLevel { get; set; } = level switch
    {
        Level.Debug => LogLevel.Debug,
        Level.Info => LogLevel.Info,
        Level.Warning => LogLevel.Warn,
        Level.Error => LogLevel.Error,
        Level.Ok => LogLevel.Debug,
        Level.Important => LogLevel.Warn,
        _ => LogLevel.Fatal,
    };

    public override string ToString() => 
        $"[{Timestamp:HH:mm:ss}] - {Level.ToString().ToUpper(CultureInfo.InvariantCulture)} " +
        $"- {Message}";

}

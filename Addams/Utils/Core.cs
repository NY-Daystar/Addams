using System;

namespace Addams.Utils;

public static class Core
{
    /// <summary>
    /// Write output with some color
    /// </summary>
    /// <param name="oo">Objects to output</param>
    public static void WriteLine(params object[] oo)
    {
        foreach (object o in oo)
        {
            if (o == null)
            {
                Console.ResetColor();
            }
            else if (o is ConsoleColor consoleColor)
            {
                Console.ForegroundColor = consoleColor;
            }
            else
            {
                Console.Write(o.ToString());
                Console.ResetColor();
            }
        }

        Console.WriteLine();
    }
}

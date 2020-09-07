using System;

namespace pdfconv
{
    public static class ColorConsole
    {
        public static void Write(string message, ConsoleColor color)
        {
            var backupColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = backupColor;
        }

        public static void WriteLine(string message, ConsoleColor color)
        {
            var backupColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = backupColor;
        }
    }
}

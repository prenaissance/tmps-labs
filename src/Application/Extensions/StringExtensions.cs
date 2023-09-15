using System.Collections.ObjectModel;

namespace Journal.Application.Extensions;

public static class StringExtensions
{
    private const string COLOR_RESET = "\u001b[0m";
    private static readonly ReadOnlyDictionary<ConsoleColor, string> _ansiColorDictionary = new(
        new Dictionary<ConsoleColor, string> {
            { ConsoleColor.Black, "\u001b[30m" },
            { ConsoleColor.DarkBlue, "\u001b[34m" },
            { ConsoleColor.DarkGreen, "\u001b[32m" },
            { ConsoleColor.DarkCyan, "\u001b[36m" },
            { ConsoleColor.DarkRed, "\u001b[31m" },
            { ConsoleColor.DarkMagenta, "\u001b[35m" },
            { ConsoleColor.DarkYellow, "\u001b[33m" },
            { ConsoleColor.Gray, "\u001b[37m" },
            { ConsoleColor.DarkGray, "\u001b[90m" },
            { ConsoleColor.Blue, "\u001b[94m" },
            { ConsoleColor.Green, "\u001b[92m" },
            { ConsoleColor.Cyan, "\u001b[96m" },
            { ConsoleColor.Red, "\u001b[91m" },
            { ConsoleColor.Magenta, "\u001b[95m" },
            { ConsoleColor.Yellow, "\u001b[93m" },
            { ConsoleColor.White, "\u001b[97m" }
        }
    );
    public static string ToColor(this string str, ConsoleColor color)
    {
        return $"{_ansiColorDictionary[color]}{str}{COLOR_RESET}";
    }
}
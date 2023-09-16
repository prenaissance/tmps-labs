using Journal.Application.Common.Utilities;

namespace Journal.Application.Extensions;

public static class StringExtensions
{
    public static string ToColor(this string str, ConsoleColor color) =>
        $"{AnsiColors.GetAnsiColor(color)}{str}{AnsiColors.GetAnsiColor(Console.ForegroundColor)}";
}
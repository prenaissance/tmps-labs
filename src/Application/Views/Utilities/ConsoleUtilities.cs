namespace Journal.Application.Views.Utilities;

public static class ConsoleUtilities
{
    public static void ResetCursor(int lineCount)
    {
        Console.CursorTop = Math.Max(Console.CursorTop - Math.Min(lineCount, Console.BufferHeight), 0);
    }
    public static void PreRender(int lineCount)
    {
        ResetCursor(lineCount);
        for (int i = 0; i < lineCount; i++)
        {
            Console.WriteLine(new string(' ', Console.BufferWidth));
        }
        ResetCursor(lineCount);
    }
}
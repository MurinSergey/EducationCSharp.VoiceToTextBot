using VoiceToTextBot.Interfaces;

namespace VoiceToTextBot.Helper;

public class Logger : ILogger
{
    public void Event(string eventMsg)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"СОБЫТИЕ: {eventMsg}");
        Console.ResetColor();
    }

    public void Warning(string warningMsg)
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine($"ПРЕДУПРЕЖДЕНИЕ: {warningMsg}");
        Console.ResetColor();
    }

    public void Error(string errorMsg)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"ОШИБКА: {errorMsg}");
        Console.ResetColor();
    }
}
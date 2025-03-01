namespace VoiceToTextBot.Controllers;

/// <summary>
/// Обработка всех не обработанных сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class DefaultMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
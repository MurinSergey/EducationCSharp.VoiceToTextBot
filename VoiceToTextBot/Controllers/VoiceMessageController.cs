namespace VoiceToTextBot.Controllers;

/// <summary>
/// Обработка аудио сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class VoiceMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
namespace VoiceToTextBot.Controllers;

public class VoiceMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
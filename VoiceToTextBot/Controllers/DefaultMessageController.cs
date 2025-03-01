namespace VoiceToTextBot.Controllers;

public class DefaultMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
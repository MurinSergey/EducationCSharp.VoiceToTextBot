namespace VoiceToTextBot.Controllers;

public class TextMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
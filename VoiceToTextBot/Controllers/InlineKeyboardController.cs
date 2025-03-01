namespace VoiceToTextBot.Controllers;

public class InlineKeyboardController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
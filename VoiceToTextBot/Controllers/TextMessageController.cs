using Telegram.Bot.Types;

namespace VoiceToTextBot.Controllers;
/// <summary>
/// Обработка текстовых сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class TextMessageController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
    
    

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        
    }
}
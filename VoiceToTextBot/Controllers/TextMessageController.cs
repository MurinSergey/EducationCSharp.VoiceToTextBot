using Telegram.Bot.Types;

namespace VoiceToTextBot.Controllers;
/// <summary>
/// Обработка текстовых сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class TextMessageController(ITelegramBotClient telegramClient, ILoggerFactory loggerFactory)
{
    private readonly ILogger<TextMessageController>? _logger = loggerFactory.CreateLogger<TextMessageController>();

    /// <summary>
    /// Обработка текста
    /// </summary>
    /// <param name="message">Тип Message из Telegram.Bot</param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("От пользователя {UserName} получено текстовое сообщение: {Message}", message.From?.Username ?? "<Неизвестный>", message.Text);
        await telegramClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Получено текстовое сообщение",
            cancellationToken: cancellationToken
            );
    }
}
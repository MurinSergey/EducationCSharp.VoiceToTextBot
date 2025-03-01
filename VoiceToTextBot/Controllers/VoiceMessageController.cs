using Telegram.Bot.Types;

namespace VoiceToTextBot.Controllers;

/// <summary>
/// Обработка голосовых сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class VoiceMessageController(ITelegramBotClient telegramClient, ILoggerFactory loggerFactory)
{
    private readonly ILogger<VoiceMessageController>? _logger = loggerFactory.CreateLogger<VoiceMessageController>();

    /// <summary>
    /// Обработка голосового сообщения
    /// </summary>
    /// <param name="message">Тип Message из Telegram.Bot</param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("От пользователя {UserName} получено голосовое сообщение", message.From?.Username ?? "<Неизвестный>");
        await telegramClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Получено голосовое сообщение",
            cancellationToken: cancellationToken
        );
    }
}
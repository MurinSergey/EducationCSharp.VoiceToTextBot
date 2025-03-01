using Telegram.Bot.Types;

namespace VoiceToTextBot.Controllers;

/// <summary>
/// Обработка всех не обработанных сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class DefaultMessageController(ITelegramBotClient telegramClient, ILoggerFactory loggerFactory)
{
    private readonly ILogger<DefaultMessageController>? _logger = loggerFactory.CreateLogger<DefaultMessageController>();

    /// <summary>
    /// Обработка всех не обработанных сообщений
    /// </summary>
    /// <param name="message">Тип Message из Telegram.Bot</param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        _logger?.LogWarning("От пользователя {UserName} получено не обрабатываемый формат сообщения: {Type}", message.From?.Username ?? "<Неизвестный>", message.Type);
        await telegramClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Получено не обрабатываемое сообщение",
            cancellationToken: cancellationToken
        );
    }
}
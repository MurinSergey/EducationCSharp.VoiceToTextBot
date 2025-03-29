using Telegram.Bot.Types;
using VoiceToTextBot.Configuration;
using VoiceToTextBot.Services;

namespace VoiceToTextBot.Controllers;

/// <summary>
/// Обработка голосовых сообщений
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class VoiceMessageController(ITelegramBotClient telegramClient, IStorage storage, IFileHandler fileHandler, ILoggerFactory loggerFactory)
{
    private readonly ILogger<VoiceMessageController>? _logger = loggerFactory.CreateLogger<VoiceMessageController>();

    /// <summary>
    /// Обработка голосового сообщения
    /// </summary>
    /// <param name="message">Тип Message из Telegram.Bot</param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var fileId = message.Voice?.FileId;
        
        _logger?.LogInformation("От пользователя {UserName} получено голосовое сообщение {fileID}", message.From?.Username ?? "<Неизвестный>", fileId);

        if (fileId is null)
        {
            return;
        }

        await fileHandler.Download(fileId, cancellationToken);
        
        
        await telegramClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Получено голосовое сообщение",
            cancellationToken: cancellationToken
        );
        
        var result = fileHandler.Process(storage.GetSession(message.Chat.Id).LangCode);
        await telegramClient.SendMessage(
            chatId: message.Chat.Id,
            text: result,
            cancellationToken: cancellationToken
        );
    }
}
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
        switch (message.Text)
        {
            case "/start":
                
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"Русский", $"ru"),
                        InlineKeyboardButton.WithCallbackData($"English", $"en"),
                        InlineKeyboardButton.WithCallbackData($"German", $"de")
                    }
                };
                
                await telegramClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: $"<b>  Наш бот превращает аудио в текст.</b> {Environment.NewLine}" + 
                            $"{Environment.NewLine}Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}",
                    cancellationToken: cancellationToken,
                    parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons)
                    );
                break;
            
            default:
                await telegramClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: "Отправьте аудио сообщение для превращения в текст",
                    cancellationToken: cancellationToken
                );
                break;
        }
    }
}
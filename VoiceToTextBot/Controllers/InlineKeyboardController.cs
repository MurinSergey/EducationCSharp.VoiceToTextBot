using Telegram.Bot.Types;

namespace VoiceToTextBot.Controllers;

/*
 * InlineKeyboardController - это условное название, которое может использоваться для описания контроллера,
 * управляющего инлайн-клавиатурами в Telegram-ботах.
 * В Telegram Bot API инлайн-клавиатуры (Inline Keyboard) — это специальные кнопки,
 * которые могут быть встроены в сообщения и позволяют пользователю взаимодействовать с ботом, не отправляя новые сообщения.
 */

/// <summary>
/// События от инлайн-клавиатуры
/// </summary>
/// <param name="telegramClient">Ссылка на телеграм-бота</param>
public class InlineKeyboardController(ITelegramBotClient telegramClient, ILoggerFactory loggerFactory)
{
    private readonly ILogger<InlineKeyboardController>? _logger = loggerFactory.CreateLogger<InlineKeyboardController>();
    
    /// <summary>
    /// Обработка инлайн-кнопок
    /// </summary>
    /// <param name="callbackQuery">Тип CallbackQuery из Telegram.Bot</param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery != null)
        {
            _logger?.LogInformation("От пользователя {UserName} получено событие инлайн-кнопки",
                callbackQuery.From.Username ?? "<Неизвестный>");
            await telegramClient.SendMessage(
                chatId: callbackQuery.From.Id,
                text: "Получено текстовое сообщение",
                cancellationToken: cancellationToken
            );
            return;
        }

        _logger?.LogWarning("Объект CallbackQuery или CallbackQuery.Message равен Null");
    }
}
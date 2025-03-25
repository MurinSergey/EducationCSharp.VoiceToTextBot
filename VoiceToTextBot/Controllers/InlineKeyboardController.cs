using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceToTextBot.Services;

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
public class InlineKeyboardController(ITelegramBotClient telegramClient, IStorage storage, ILoggerFactory loggerFactory)
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
            _logger?.LogInformation("От пользователя {UserName} получено событие инлайн-кнопки: {Data}", callbackQuery.From.Username ?? "<Неизвестный>", callbackQuery.Data);
            // Сохраняем выбор языка для текущего чата
            storage.GetSession(callbackQuery.From.Id).LangCode = callbackQuery.Data;
            
            // Создаем сообщение
            var lang = callbackQuery.Data switch
            {
                "ru" => "Русский",
                "en" => "English",
                _ => string.Empty
            };
            
            await telegramClient.SendMessage(
                chatId: callbackQuery.From.Id,
                text: $"<b>Язык аудио - {lang}.</b>{Environment.NewLine}" +
                        $"{Environment.NewLine}Можно поменять в главном меню.",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken
            );
            return;
        }

        _logger?.LogWarning("Объект CallbackQuery или CallbackQuery.Message равен Null");
    }
}
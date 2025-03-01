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
public class InlineKeyboardController(ITelegramBotClient telegramClient)
{
    private readonly ITelegramBotClient _telegramClient = telegramClient;
}
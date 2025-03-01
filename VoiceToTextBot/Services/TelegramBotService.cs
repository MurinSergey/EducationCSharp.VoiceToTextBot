using Microsoft.Extensions.Hosting;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceToTextBot.Controllers;

namespace VoiceToTextBot.Services;

internal class TelegramBotService : BackgroundService
{
    /// <summary>
    /// Ссылка на объект бота
    /// </summary>
    private readonly ITelegramBotClient _telegramClient;
    
    private readonly ILogger<TelegramBotService>? _loggerFactory;
    
    /// <summary>
    /// Контроллер не обработанных сообщений
    /// </summary>
    private readonly DefaultMessageController _defaultMessageController;
    
    /// <summary>
    /// Контроллер обработки текста
    /// </summary>
    private readonly TextMessageController _textMessageController;
    
    /// <summary>
    /// Контроллер обработки голосовых сообщений
    /// </summary>
    private readonly VoiceMessageController _voiceMessageController;
    
    /// <summary>
    /// Контроллер инлайн-клавиатуры
    /// </summary>
    private readonly InlineKeyboardController _inlineKeyboardController;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="telegramClient">Ссылка на внешний объект бота</param>
    /// <param name="loggerFactory"></param>
    /// <param name="defaultMessageController"></param>
    /// <param name="textMessageController"></param>
    /// <param name="voiceMessageController"></param>
    /// <param name="inlineKeyboardController"></param>
    public TelegramBotService(
        ITelegramBotClient telegramClient, 
        ILoggerFactory loggerFactory, 
        DefaultMessageController defaultMessageController, 
        TextMessageController textMessageController, 
        VoiceMessageController voiceMessageController, 
        InlineKeyboardController inlineKeyboardController)
    {
        _telegramClient = telegramClient;
        _defaultMessageController = defaultMessageController;
        _textMessageController = textMessageController;
        _voiceMessageController = voiceMessageController;
        _inlineKeyboardController = inlineKeyboardController;
        _loggerFactory = loggerFactory.CreateLogger<TelegramBotService>();
    }
    
    /// <summary>
    /// Запуск кода как фоновый сервис
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() {AllowedUpdates = []}, // Здесь выбираем, какие обновления хотим получать. В данном случае - разрешены все
            cancellationToken: stoppingToken);

        _loggerFactory?.LogInformation("Бот запущен");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(1000, stoppingToken);
            }
            catch (TaskCanceledException ex)
            {
                _loggerFactory?.LogWarning("Задача была прервана: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                // Выводим сообщение в консоль
                _loggerFactory?.LogError("Выполнение задачи: {Message}", ex.Message);
            }
        }
    }

    /// <summary>
    /// Реакция на события в чате (кнопки, сообщения...)
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="update"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update)
        {
            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            case { Type: UpdateType.Message, Message: not null }:
                switch (update.Message.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        break;
                    case MessageType.Voice:
                        await _voiceMessageController.Handle(update.Message, cancellationToken);
                        break;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        break;
                }
                return;
            
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            case { Type: UpdateType.CallbackQuery}:
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            
            default:
                _loggerFactory?.LogWarning("Необрабатываемый тип сообщения от пользователя: {Type}", update.Type);
                return;
        }
    }

    /// <summary>
    /// Обработчик ошибок
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMsg = exception switch
        {
            ApiRequestException apiRequestException => $"ОШИБКА Telegram API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => $"ОШИБКА:\n{exception.Message}"
        };

        // Выводим сообщение в консоль
        _loggerFactory?.LogError("{Message}", errorMsg);
        
        // Метод await Task.Delay(10000, cancellationToken) используется для создания асинхронной задержки (паузы) в выполнении кода на указанное время (в данном случае — 10 секунд), 
        // с возможностью отмены этой задержки с помощью CancellationToken.
        try
        {
            // Повторное подключение
            _loggerFactory?.LogInformation("Повторное подключение");
            await Task.Delay(10000, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            _loggerFactory?.LogWarning("Задача была прервана: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            // Выводим сообщение в консоль
            _loggerFactory?.LogError("Выполнение задачи: {Message}", ex.Message);
        }
    }
}

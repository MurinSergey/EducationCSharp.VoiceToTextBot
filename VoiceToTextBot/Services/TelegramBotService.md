### Про класс TelegramBot

В курсе старая информация, поэтому дорабатываем.

#### Основные изменения метода HandleUpdateAsync:

    Другой метод: Метод SendTextMessageAsync был перименован в SendMessage согласно документации Telegram.Bot

    Проверка на null: В последней версии библиотеки свойства CallbackQuery и Message могут быть null, поэтому добавлены проверки на null.

    Использование update.CallbackQuery: Вместо update.Message.Chat.Id используется update.CallbackQuery.Message.Chat.Id, так как CallbackQuery содержит свойство Message, которое ссылается на сообщение, к которому привязана кнопка.

    Использование update.Message: Для обработки входящих сообщений используется update.Message, и проверяется, что оно не null.

Код из урока:

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, "Вы нажали кнопку", cancellationToken: cancellationToken);
            return;
        }
    
        // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
        if (update.Type == UpdateType.Message)
        {
            await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, "Вы отправили сообщение", cancellationToken: cancellationToken);
            return;
        }
    }

### Основные изменения метода HandleErrorAsync:

    1. **Асинхронная задержка**: Вместо использования `Thread.Sleep`, который блокирует поток, используется `await Task.Delay`, что позволяет асинхронно ожидать без блокировки потока. Это особенно важно в асинхронных приложениях, чтобы не блокировать поток и не снижать производительность.

    2. **Использование `CancellationToken`**: Добавлен параметр `cancellationToken` в метод `Task.Delay`, чтобы можно было отменить задержку, если это потребуется. Это хорошая практика для асинхронных методов, чтобы они могли корректно реагировать на запросы отмены.

    3. **Упрощение возврата**: Вместо возврата `Task.CompletedTask`, метод теперь просто завершается с `return`, так как он объявлен как `async` и автоматически возвращает `Task`.

Код из урока:

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
 
        // Выводим в консоль информацию об ошибке
        Console.WriteLine(errorMessage);
 
        // Задержка перед повторным подключением
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
        Thread.Sleep(10000);
 
        return Task.CompletedTask;
    }

Метод `await Task.Delay(10000, cancellationToken)` используется для создания асинхронной задержки (паузы) в выполнении кода на указанное время (в данном случае — 10 секунд), с возможностью отмены этой задержки с помощью `CancellationToken`.

#### Разберем по частям:

    1. **`Task.Delay`**:
       - Это метод, который создает задачу (`Task`), которая завершается через указанное количество миллисекунд. В данном случае `10000` миллисекунд = 10 секунд.
       - Это асинхронный метод, который не блокирует поток, в отличие от `Thread.Sleep`.

    2. **`await`**:
       - Ключевое слово `await` приостанавливает выполнение текущего метода до тех пор, пока задача (`Task`), возвращаемая `Task.Delay`, не завершится.
       - Во время ожидания поток не блокируется, и система может использовать его для выполнения других задач.

    3. **`cancellationToken`**:
       - Это параметр, который позволяет отменить выполнение задержки, если это потребуется.
       - Если `cancellationToken` будет отменен (например, вызван метод `Cancel()` на объекте `CancellationTokenSource`), задача `Task.Delay` немедленно завершится с исключением `OperationCanceledException`.

#### Как это работает:

    - Когда выполнение кода доходит до `await Task.Delay(10000, cancellationToken)`, метод приостанавливается на 10 секунд.
    - Если в течение этих 10 секунд `cancellationToken` будет отменен, задержка прерывается, и метод продолжает выполнение с обработкой исключения `OperationCanceledException`.
    - Если задержка завершается успешно (без отмены), выполнение метода продолжается со следующей строки.

#### Пример использования:

    ```csharp
    public async Task DoSomethingAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Начало работы.");

        try
        {
            // Ожидаем 10 секунд с возможностью отмены
            await Task.Delay(10000, cancellationToken);
            Console.WriteLine("10 секунд прошли успешно.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Задержка была отменена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("Завершение работы.");
    }
    ```

#### Когда это полезно:

    1. **Асинхронные паузы**:
       - Например, если вы хотите сделать паузу между запросами к API, чтобы не превысить лимиты запросов.

    2. **Отмена операций**:
       - Если пользователь или система решает прервать операцию, `cancellationToken` позволяет немедленно остановить задержку и продолжить выполнение кода.

    3. **Эффективное использование ресурсов**:
       - В отличие от `Thread.Sleep`, который блокирует поток, `Task.Delay` освобождает поток для выполнения других задач, что особенно важно в асинхронных приложениях.
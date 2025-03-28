
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;using VoiceToTextBot.Configuration;
using VoiceToTextBot.Controllers;
using VoiceToTextBot.Extensions;
using VoiceToTextBot.Services;

Console.OutputEncoding = Encoding.Unicode;

Console.WriteLine(DirectoryExtension.GetSolutionRoot());

// Создаем фабрику логерров
var loggerFactory = LoggerFactory.Create((builder) =>
{
    builder.AddConsole(); // Добавляем работу с консолью
});

// Логгер основной программы
var mainLogger = loggerFactory.CreateLogger<Program>();

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("botconfig.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) => ConfigureServices(services, hostContext.Configuration, loggerFactory))
    .UseConsoleLifetime()
    .Build();

mainLogger.LogInformation("Сервис запущен");

await host.RunAsync();

mainLogger.LogInformation("Сервис остановлен");

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
{
    // Создаем объект с настройками бота и регистрируем
    var appSettings = BuildAppSettings(configuration);
    services.AddSingleton(appSettings);
    
    // Регистрируем объект фабрики логгеров (один экземпляр)
    services.AddSingleton(loggerFactory);

    // Регистрация хранилища сессии
    services.AddSingleton<IStorage, MemoryStorage>();

    // Регистрация обработчика голосовых сообщений
    services.AddSingleton<IFileHandler, AudioFileHandler>();

    // Регистрируем контроллеры (будут создаваться каждый раз при необходимости)
    services.AddTransient<DefaultMessageController>();
    services.AddTransient<TextMessageController>();
    services.AddTransient<VoiceMessageController>();
    services.AddTransient<InlineKeyboardController>();
    
    //Регистрируем объект TelegramBotClient c токеном подключения (один экземпляр)
    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token: appSettings.BotToken));
    
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<TelegramBotService>();
}

// Создает объект с настройками бота 
static AppSettings BuildAppSettings(IConfiguration configuration)
{
    return new AppSettings()
    {
        BotToken = configuration["TelegramToken"],
        DownloadsFolder = configuration["DownloadsFolder"],
        AudioFileName = configuration["AudioFileName"],
        AudioFileFormat = configuration["AudioFileFormat"],
        ConvertAudioFormat = configuration["ConvertAudioFormat"]
    };
}
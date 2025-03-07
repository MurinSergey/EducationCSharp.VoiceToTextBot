
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VoiceToTextBot.Controllers;
using VoiceToTextBot.Services;

Console.OutputEncoding = Encoding.Unicode;

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
        config.AddJsonFile("botconfig.json", optional: true, reloadOnChange: true);
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
    var token = configuration["TelegramToken"];
    
    // Регистрируем объект фабрики логгеров (один экземпляр)
    services.AddSingleton(loggerFactory);

    // Регистрируем контроллеры (будут создаваться каждый раз при необходимости)
    services.AddTransient<DefaultMessageController>();
    services.AddTransient<TextMessageController>();
    services.AddTransient<VoiceMessageController>();
    services.AddTransient<InlineKeyboardController>();
    
    //Регистрируем объект TelegramBotClient c токеном подключения (один экземпляр)
    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token: token));
    
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<TelegramBotService>();
}
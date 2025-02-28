
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceToTextBot.Helper;
using VoiceToTextBot.Interfaces;
using VoiceToTextBot.Services;

Console.OutputEncoding = Encoding.Unicode;

var logger = new Logger();

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("botconfig.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) => ConfigureServices(services, hostContext.Configuration))
    .UseConsoleLifetime()
    .Build();

logger.Event("Сервис запущен");

await host.RunAsync();

logger.Event("Сервис остановлен");

return;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var token = configuration["TelegramToken"];
    
    //Регистрируем объект TelegramBotClient c токеном подключения
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(token: token));

    services.AddSingleton<ILogger>(new Logger());
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<TelegramBotService>();
}
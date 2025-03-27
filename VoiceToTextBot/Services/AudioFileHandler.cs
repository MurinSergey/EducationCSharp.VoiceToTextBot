using VoiceToTextBot.Configuration;

namespace VoiceToTextBot.Services;


/// <summary>
/// Класс для загрузки и обработки голосовых сообщений
/// </summary>
/// <param name="telegramBotClient"></param>
/// <param name="settings"></param>
public class AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings settings, ILoggerFactory loggerFactory) : IFileHandler
{

    /// <summary>
    /// Экземпляр логирования
    /// </summary>
    private readonly ILogger<AudioFileHandler>? _logger = loggerFactory.CreateLogger<AudioFileHandler>(); 
    
    /// <summary>
    /// Загрузка аудио файлов
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Download(string fileId, CancellationToken ct)
    {
        // Полный путь для загрузки файла
        var downloadPath = Path.Combine(settings.DownloadsFolder,
            $"{fileId}-{settings.AudioFileName}.{settings.AudioFileFormat}");
        
        // Процесс загрузки файла
        await using var audioFileStream = File.Create(downloadPath);
        _logger?.LogInformation("Загружаем аудио файл в {path}", downloadPath);
        var fileInfo = await telegramBotClient.GetInfoAndDownloadFile(fileId, audioFileStream, ct);
        _logger?.LogInformation("Загружен файл {downloadPath} размером {info}", downloadPath, fileInfo.FileSize);
    }

    public string Process(string param)
    {
        throw new NotImplementedException();
    }
}
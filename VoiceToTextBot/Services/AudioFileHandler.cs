using VoiceToTextBot.Configuration;
using VoiceToTextBot.Utilities;

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
    /// Загрузка голосовых сообщений
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="ct"></param>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Download(string fileId, CancellationToken ct)
    {
        // Проверяем входные данные fileID
        if (string.IsNullOrWhiteSpace(fileId))
        {
            _logger?.LogError("Аргумент fileID не может быть пустым");
            throw new AggregateException("Аргумент fileID не может быть пустым");
        }
        
        // Проверяем конфигурацию
        if (string.IsNullOrWhiteSpace(settings.DownloadsFolder))
        {
            _logger?.LogError("Путь сохранения файлов не задан в конфигурации приложения");
            throw new InvalidOperationException("Сохранение не возможно, путь сохранения файлов не задан в конфигурации приложения");
        }
        
        // Обрабатываем отмену задания
        ct.ThrowIfCancellationRequested();

        // Создаем папку сохранения файлов если её нет
        if (!Directory.Exists(settings.DownloadsFolder))
        {
            Directory.CreateDirectory(settings.DownloadsFolder);
        }
        
        // Полный путь для загрузки файла
        var downloadPath = Path.Combine(settings.DownloadsFolder,
            $"{settings.AudioFileName}.{settings.AudioFileFormat}");
        
        // Процесс загрузки файла
        await using var audioFileStream = File.Create(downloadPath);

        // Защищенный способ загрузки файлов
        try
        {
            _logger?.LogInformation("Загружаем аудио файл в {path}", downloadPath);
            var fileInfo = await telegramBotClient.GetInfoAndDownloadFile(fileId, audioFileStream, ct);
            _logger?.LogInformation("Загружен файл {downloadPath} размером {info}", downloadPath, fileInfo.FileSize);
        }
        catch (OperationCanceledException)
        {
            _logger?.LogWarning("Загрузка файла {fileId} отменена", fileId);
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Ошибка при загрузке файла {fileId}", fileId);
            throw;
        }
    }

    /// <summary>
    /// Обработка голосовых сообщений
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string Process(string param)
    {
        var inputFilePath =
            Path.Combine(settings.DownloadsFolder, $"{settings.AudioFileName}.{settings.AudioFileFormat}");
        var outFilePath = Path.Combine(settings.DownloadsFolder,
            $"{settings.AudioFileName}.{settings.ConvertAudioFormat}");
        
        _logger?.LogInformation("Запуск конвертации файла");
        AudioConverter.TryConvert(inputFilePath, outFilePath);
        _logger?.LogInformation("Конвертация завершена");
        return "Конвертация завершена";
    }
}
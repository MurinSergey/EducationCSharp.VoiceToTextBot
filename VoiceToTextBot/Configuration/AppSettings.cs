namespace VoiceToTextBot.Configuration;

public class AppSettings
{
    /// <summary>
    /// Токен бота
    /// </summary>
    public string BotToken { get; set; }
    
    /// <summary>
    /// Директория для загрузки файлов
    /// </summary>
    public string DownloadsFolder { get; set; }
    
    /// <summary>
    /// Имя аудио файлов
    /// </summary>
    public string AudioFileName { get; set; }
    
    /// <summary>
    /// Формат аудио файлов
    /// </summary>
    public string AudioFileFormat { get; set; }
    
    /// <summary>
    /// Формат в который необходимо конвертировать файл
    /// </summary>
    public string ConvertAudioFormat { get; set; }
    
    /// <summary>
    /// Битрейт аудио файла
    /// </summary>
    public float InputAudioBitrate { get; set; }
}
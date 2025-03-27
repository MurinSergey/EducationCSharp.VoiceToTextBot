namespace VoiceToTextBot.Services;

public interface IFileHandler
{
    /// <summary>
    /// Метод для загрузки файлов из телеграм бота
    /// </summary>
    /// <param name="fileId">Уникальный id для каждого файла</param>
    /// <param name="ct">Токен для отмены задачи</param>
    /// <returns>Возвращает задачу, которая может выполнять асинхронно</returns>
    Task Download(string fileId, CancellationToken ct);

    /// <summary>
    /// Метод для обработки файла (распознавание)
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    string Process(string param);

}
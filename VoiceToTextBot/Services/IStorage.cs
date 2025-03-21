using VoiceToTextBot.Models;

namespace VoiceToTextBot.Services;

public interface IStorage
{
    /// <summary>
    /// Получение сессии по id чата
    /// </summary>
    /// <param name="chatId">ID чата</param>
    /// <returns>Искомая сессия</returns>
    Session GetSession(long chatId);
}
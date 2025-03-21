using System.Collections.Concurrent;
using VoiceToTextBot.Models;

namespace VoiceToTextBot.Services;

public class MemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions;

    public MemoryStorage()
    {
        _sessions = new ConcurrentDictionary<long, Session>();
    }

    /// <summary>
    /// Получение сессии по id чата
    /// </summary>
    /// <param name="chatId">ID чата</param>
    /// <returns>Искомая сессия</returns>
    public Session GetSession(long chatId)
    {
        return _sessions.GetOrAdd(chatId, CreateDefaultSession(chatId));
    }

    /// <summary>
    /// Создается новая сессия с кодом языка по умолчанию
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    private Session CreateDefaultSession(long chatId)
    {
        return new Session() { LangCode = "ru" };
    }
}
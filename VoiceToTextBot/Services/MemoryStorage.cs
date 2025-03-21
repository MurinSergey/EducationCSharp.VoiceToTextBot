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
        if (_sessions.TryGetValue(chatId, out var session))
        {
            return session;
        }
        
        var newSession = new Session(){ LangCode = "ru"};
        _sessions.TryAdd(chatId, newSession);
        return newSession;
    }
}
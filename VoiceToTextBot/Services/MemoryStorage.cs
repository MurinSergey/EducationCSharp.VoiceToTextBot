using System.Collections.Concurrent;
using VoiceToTextBot.Models;

namespace VoiceToTextBot.Services;

public class MemoryStorage(ILoggerFactory loggerFactory) : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions = new();

    private readonly ILogger<MemoryStorage>? _logger = loggerFactory.CreateLogger<MemoryStorage>();

    /// <summary>
    /// Получение сессии по id чата
    /// </summary>
    /// <param name="chatId">ID чата</param>
    /// <returns>Искомая сессия</returns>
    public Session GetSession(long chatId)
    {
        var session = _sessions.GetOrAdd(chatId, CreateDefaultSession(chatId));
        _logger?.LogInformation("Сессия для {chat} загружена. Язык: {session}",chatId, session.LangCode);
        return session;
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
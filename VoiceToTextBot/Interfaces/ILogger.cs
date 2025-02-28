namespace VoiceToTextBot.Interfaces;

public interface ILogger
{
    void Event(string eventMsg);
    
    void Warning(string warningMsg);
    
    void Error(string errorMsg);
}
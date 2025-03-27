namespace VoiceToTextBot.Extensions;

/// <summary>
/// Расширение класса String
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Методо возвращает строку с заглавной буквой
    /// </summary>
    /// <param name="s">Строка</param>
    /// <returns>Строка с заглавной буквой</returns>
    public static string UppercaseFirst(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        return char.ToUpper(s[0]) + s[1..];
    }
}
namespace VoiceToTextBot.Extensions;

/// <summary>
/// Класс расширения для получения пути
/// </summary>
public static class DirectoryExtension
{
    /// <summary>
    /// Возвращает путь до директории, где лежит файл решения .sln
    /// </summary>
    /// <returns></returns>
    public static string? GetSolutionRoot()
    {
        // Пусть текущая директория: "C:\Projects\MyApp\bin\Debug\net9.0"
        var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory()); // dir = "C:\Projects\MyApp\bin\Debug"
        
        var projectPath = Directory.GetParent(dir).FullName; // projectPath = "C:\Projects\MyApp\bin"
        
        var projectRoot = projectPath[..^4]; // projectRoot = "C:\Projects\MyApp"
        
        return Directory.GetParent(projectPath)?.FullName; // return = "C:\Projects"
    }
}
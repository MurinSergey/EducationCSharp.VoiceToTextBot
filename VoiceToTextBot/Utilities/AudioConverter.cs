using FFMpegCore;
using VoiceToTextBot.Extensions;

namespace VoiceToTextBot.Utilities;

/// <summary>
/// Используем программу ffmpeg для конвертации голосового сообщения
/// </summary>
public static class AudioConverter
{
    public static void TryConvert(string inputFile, string outputFile)
    {
        // Задаём путь, где лежит вспомогательная программа - конвертер
        GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(DirectoryExtension.GetSolutionRoot(), ".speech-detect", "ffmpeg-win64", "bin"));

        // Вызываем Ffmpeg, передав требуемые аргументы.
        FFMpegArguments
            .FromFileInput(inputFile)
            .OutputToFile(outputFile, true, options => options
                .WithFastStart())
            .ProcessSynchronously();
    }
}
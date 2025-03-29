using System.Text;
using Newtonsoft.Json.Linq;
using VoiceToTextBot.Extensions;
using Vosk;

namespace VoiceToTextBot.Utilities;

public static class SpeechDetector
{
    public static string DetectSpeech(string audioPath, float inputBitrate, string languageCode)
    {
        Vosk.Vosk.SetLogLevel(-1); // -1 отключает логи, 0 включает логи
        var modelPath = Path.Combine(DirectoryExtension.GetSolutionRoot(), "Speech-models", $"vosk-model-small-{languageCode.ToLower()}");
        Model model = new(modelPath);
        return GetWords(model, audioPath, inputBitrate);
    }

    /// <summary>
    /// Основной метод для распознавания слов
    /// </summary>
    private static string GetWords(Model model, string audioPath, float inputBitrate)
    {
       
        // В конструктор для распознавания передаем битрейт, а также используемую языковую модель
        VoskRecognizer rec = new(model, inputBitrate);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);

        StringBuilder textBuffer = new();

        using (Stream source = File.OpenRead(audioPath))
        {
            byte [] buffer = new byte[4096];
            int bytesRead;
            
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Распознавание отдельных слов
                var accepted = rec.AcceptWaveform(buffer, bytesRead);
                // Console.WriteLine($">>>>>>>{accepted} BytesRead = {bytesRead}");
                if (accepted)
                {
                    var sentenceJson = rec.Result();
                    // Сохраняем текстовый вывод в JSON-объект и извлекаем данные
                    var sentenceObj = JObject.Parse(sentenceJson);
                    var sentence = (string)sentenceObj["text"];
                    textBuffer.Append(sentence.UppercaseFirst() + ". ");
                }
            }
        }

        // Распознавание предложений
        var finalSentence = rec.FinalResult();
        // Сохраняем текстовый вывод в JSON-объект и извлекаем данные
        JObject finalSentenceObj = JObject.Parse(finalSentence);

        // Собираем итоговый текст
        textBuffer.Append((string)finalSentenceObj["text"]);
        // Возвращаем в виде строки
        return textBuffer.ToString();
    }
}
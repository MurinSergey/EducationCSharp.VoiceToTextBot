# Учебный проект телеграм бота

## Задача бота

    Преобразовывать аудио дорожку в текст

## Зависимости

    * Telegram.Bot - установлен командой <dotnet add package Telegram.Bot --version 22.5.0-dev.8>
    * Необходима программа FFmpeg
    * Необходимы модели для Vosk

## Настройки

    В корне проекта VoiceToTexBot необходимо создать файл botconfig.json и разместить там токен телеграм бота:
    {
        "TelegramToken" : "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        "DownloadsFolder": "D:\\Download",
        "AudioFileName": "audio_message",
        "AudioFileFormat": "ogg",
        "ConvertAudioFormat": "wav",
        "InputAudioBitrate": 48000
    }
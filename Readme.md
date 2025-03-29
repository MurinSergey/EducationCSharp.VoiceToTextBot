# Учебный проект телеграм бота

## Задача бота

    Преобразовывать аудио дорожку в текст

## Зависимости

    * Telegram.Bot - установлен командой <dotnet add package Telegram.Bot --version 22.5.0-dev.8>
    * Необходима программа FFmpeg
    * Необходимы модели для Vosk

Необходимо создать в корне решения (рядом с файлом .sln) директорию .speech-detect и туда сложить содержимое архивов:
* FFmpeg-win64
* Speech-models

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
﻿using GusMelfordBot.Infrastructure;

namespace GusMelfordBot.Api.Settings;

public class AppSettings
{
    public TelegramBotSettings TelegramBotSettings { get; set; }
    public KafkaSettings KafkaSettings { get; set; }
    public DatabaseSettings DatabaseSettings { get; set; }
}
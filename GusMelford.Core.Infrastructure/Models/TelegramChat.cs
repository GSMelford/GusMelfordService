﻿namespace GusMelfordBot.Infrastructure.Models;

public class TelegramChat : BaseEntity
{
    public Application Application { get; set; }
    public long ChatId { get; set; }
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}
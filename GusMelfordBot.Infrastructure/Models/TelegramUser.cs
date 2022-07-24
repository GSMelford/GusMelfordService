﻿namespace GusMelfordBot.Infrastructure.Models;

public class TelegramUser : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string? Username { get; set; }
    public long TelegramId { get; set; }
}
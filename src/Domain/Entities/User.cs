// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace Domain.New.Entities;

public class User
{
    public string UserId { get; set; } = null!;
    public string? Gmail { get; set; }
    public long TelegramId { get; set; }
    public string? TableUrl { get; set; }
    public Settings Settings { get; set; }
}
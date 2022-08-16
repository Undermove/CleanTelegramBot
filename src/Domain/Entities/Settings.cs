namespace Domain.New.Entities;

public class Settings
{
    public string SettingsId { get; set; }
    public string UserId { get; set; }
    public int TimeZone { get; set; }
    public User User { get; set; }
}
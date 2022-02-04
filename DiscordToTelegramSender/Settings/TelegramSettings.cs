namespace DiscordToTelegramSender.Settings;

public record TelegramSettings
{
    public const string Section = "Telegram";
    
    public string Token { get; init; }
}
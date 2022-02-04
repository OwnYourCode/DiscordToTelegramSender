namespace DiscordToTelegramSender.Services.Discord;

public interface IDiscordService
{
    Task StartDiscord();

    Task Finish();
}
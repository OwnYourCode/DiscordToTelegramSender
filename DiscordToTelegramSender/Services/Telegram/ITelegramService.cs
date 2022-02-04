namespace DiscordToTelegramSender.Services.Telegram;

public interface ITelegramService
{
    Task StartReceiving();

    Task Send(string message);

    void StopAllEvents();
}
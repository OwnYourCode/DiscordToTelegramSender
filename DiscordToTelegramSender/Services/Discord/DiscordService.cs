using Discord;
using Discord.WebSocket;
using DiscordToTelegramSender.Services.Telegram;
using DiscordToTelegramSender.Settings;
using Microsoft.Extensions.Options;

namespace DiscordToTelegramSender.Services.Discord;

public class DiscordService : IDiscordService
{
    private readonly DiscordSocketClient _client;
    private readonly IDiscordCommandsContainer _discordCommandsContainer;
    private readonly ILogger<DiscordService> _logger;
    private readonly IOptions<DiscordSettings> _optionsSnapshot;
    private readonly ITelegramService _telegramService;
    private bool _isStarted;

    public DiscordService(ITelegramService telegramService,
                          DiscordSocketClient client,
                          IDiscordCommandsContainer discordCommandsContainer,
                          IOptions<DiscordSettings> optionsSnapshot,
                          ILogger<DiscordService> logger)
    {
        _telegramService = telegramService;
        _client = client;
        _discordCommandsContainer = discordCommandsContainer;
        _optionsSnapshot = optionsSnapshot;
        _logger = logger;

        AssignEvents();
    }

    public async Task StartDiscord()
    {
        if (_isStarted)
        {
            return;
        }

        await _discordCommandsContainer.LoadCommands();
        await _discordCommandsContainer.LoadModules();

        await _telegramService.StartReceiving();

        await _client.LoginAsync(TokenType.Bot, _optionsSnapshot.Value.Token);
        await _client.StartAsync();

        _isStarted = true;
    }

    public Task Finish()
    {
        _telegramService.StopAllEvents();
        return _client.StopAsync();
    }

    private void AssignEvents()
    {
        _client.Log += Log;
        _client.MessageReceived += Message;
    }

    private Task Log(LogMessage msg)
    {
        _logger.LogInformation(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task Message(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message)
        {
            return;
        }

        // var argPos = 0;
        // if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot)
        // {
        //     return;
        // }

        await _telegramService.Send($"New message from discord: {message.Content}");

        // var context = new SocketCommandContext(_client, message);
        // await _discordCommandsContainer.Execute(context, argPos, null);
    }
}
using DiscordToTelegramSender.Services.Discord;
using DiscordToTelegramSender.Settings;
using Microsoft.Extensions.Options;

namespace DiscordToTelegramSender.Services.Background;

public class PipelineRunner : BackgroundService
{
    private readonly IDiscordService _discordService;
    private readonly ILogger<PipelineRunner> _logger;
    private readonly IOptions<DiscordSettings> _options;

    public PipelineRunner(ILogger<PipelineRunner> logger, IOptions<DiscordSettings> options, IDiscordService discordService)
    {
        _discordService = discordService;
        _logger = logger;
        _options = options;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service was started. Token: {Token}", _options.Value?.Token);
        return _discordService.StartDiscord();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service was finished");
        return _discordService.Finish();
    }
}
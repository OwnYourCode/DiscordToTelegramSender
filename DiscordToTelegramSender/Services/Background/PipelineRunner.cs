using DiscordToTelegramSender.Services.Discord;

namespace DiscordToTelegramSender.Services.Background;

public class PipelineRunner : BackgroundService
{
    private readonly IDiscordService _discordService;
    private readonly PingService _pingService;
    private readonly ILogger<PipelineRunner> _logger;

    public PipelineRunner(ILogger<PipelineRunner> logger, IDiscordService discordService, PingService pingService)
    {
        _discordService = discordService;
        _pingService = pingService;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discordService.StartDiscord();
        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _pingService.Ping();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service was finished");
        return _discordService.Finish();
    }
}
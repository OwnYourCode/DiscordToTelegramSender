using DiscordToTelegramSender.Services.Discord;

namespace DiscordToTelegramSender.Services.Background;

public class PipelineRunner : BackgroundService
{
    private readonly IDiscordService _discordService;
    private readonly ILogger<PipelineRunner> _logger;

    public PipelineRunner(ILogger<PipelineRunner> logger, IDiscordService discordService)
    {
        _discordService = discordService;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discordService.StartDiscord();
        await base.StartAsync(cancellationToken);
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
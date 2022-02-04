using System.Reflection;
using Discord.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using IResult = Discord.Commands.IResult;

namespace DiscordToTelegramSender.Services.Telegram;

public class DiscordCommandsContainer : IDiscordCommandsContainer
{
    private readonly ITelegramBotClient _client;

    private readonly CommandService _commandService = new();

    public DiscordCommandsContainer(ITelegramBotClient client)
    {
        _client = client;
    }

    public BotCommand[] Commands { get; private set; }

    public async Task LoadCommands()
    {
        Commands ??= await _client.GetMyCommandsAsync();
    }

    public Task LoadModules()
    {
        return _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), null);
    }

    public Task<IResult> Execute(ICommandContext context,
                                 string input,
                                 IServiceProvider services,
                                 MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception)
    {
        return _commandService.ExecuteAsync(context, input, services, multiMatchHandling);
    }

    public Task<IResult> Execute(ICommandContext context,
                                 int argPos,
                                 IServiceProvider services,
                                 MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception)
    {
        return _commandService.ExecuteAsync(context, argPos, services, multiMatchHandling);
    }
}
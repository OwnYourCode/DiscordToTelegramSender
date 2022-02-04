using Discord.Commands;
using Telegram.Bot.Types;
using IResult = Discord.Commands.IResult;

namespace DiscordToTelegramSender.Services.Telegram;

public interface IDiscordCommandsContainer
{
    BotCommand[] Commands { get; }

    Task LoadCommands();

    Task LoadModules();

    Task<IResult> Execute(ICommandContext context,
                          string input,
                          IServiceProvider services,
                          MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception);

    Task<IResult> Execute(ICommandContext context,
                          int argPos,
                          IServiceProvider services,
                          MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception);
}
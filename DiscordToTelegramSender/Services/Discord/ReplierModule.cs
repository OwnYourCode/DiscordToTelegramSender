using Discord.Commands;

namespace DiscordToTelegramSender.Services.Discord;

public class ReplierModule : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("echo")]
    public Task Say([Remainder] [Summary("test bot")] string echo)
    {
        Console.WriteLine($"saying {echo}");
        return ReplyAsync(echo ?? "test bot");
    }
}
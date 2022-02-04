using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DiscordToTelegramSender.Services.Telegram;

public class TelegramService : ITelegramService
{
    //TODO: Need to figure out where to save chats info (files, resources etc.) 
    private static readonly Dictionary<long, string> Chats = new();
    
    private readonly ITelegramBotClient _client;
    private readonly ILogger<TelegramService> _logger;
    private readonly CancellationTokenSource _source = new();

    public TelegramService(ITelegramBotClient client, ILogger<TelegramService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task StartReceiving()
    {
        var me = await _client.GetMeAsync();
        _logger.LogInformation("Start receiving message from {Username}", me.Username);

        _client.StartReceiving(HandleUpdate,
                               HandleError,
                               new ReceiverOptions(),
                               _source.Token);
    }

    public async Task Send(string message)
    {
        foreach (var (chatId, userName) in Chats)
        {
            await _client.SendTextMessageAsync(chatId.ToString(), message ?? $"Test message for {userName}.");
        }
    }

    public void StopAllEvents()
    {
        _source.Cancel();
    }

    private static async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        {
            return;
        }

        var (isCommand, command) = IsCommand(update.Message);

        var chatId = update.Message.Chat.Id;
        Chats.TryAdd(chatId, update.Message.From!.Username);

        if (isCommand)
        {
            Console.WriteLine($"Received a '{command}' message in chat {chatId}.");
            await HandleCommand(botClient, command, chatId, update.Message.From.Username);
            return;
        }

        var messageText = update.Message.Text;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        // Echo received message text
        await botClient.SendTextMessageAsync(chatId,
                                             "You said:\n" + messageText,
                                             cancellationToken: cancellationToken);
    }

    private static (bool IsCommand, string CommandName) IsCommand(Message message)
    {
        if (message.Entities is null || message.EntityValues is null)
        {
            return (false, null);
        }

        var entities = message.Entities.Zip(message.EntityValues,
                                            (q, w) => new
                                                      {
                                                          Entity = q,
                                                          Value = w
                                                      });
        var command = entities.FirstOrDefault(q => q.Entity.Type == MessageEntityType.BotCommand);
        return command is not null ? (true, command.Value) : (false, null);
    }

    private static async Task HandleCommand(ITelegramBotClient botClient, string command, long chatId, string userName)
    {
        switch (command)
        {
            case "/subscribe":
                await botClient.SendTextMessageAsync(chatId.ToString(), "You was subscribed!");
                await botClient.SendTextMessageAsync(chatId.ToString(), $"Test message for {userName}.");
                break;
        }
    }

    private static Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
                           {
                               ApiRequestException apiRequestException
                                   => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                               _ => exception.ToString()
                           };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}
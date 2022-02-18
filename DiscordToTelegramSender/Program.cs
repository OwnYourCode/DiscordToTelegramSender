using Discord.WebSocket;
using DiscordToTelegramSender;
using DiscordToTelegramSender.Services.Background;
using DiscordToTelegramSender.Services.Discord;
using DiscordToTelegramSender.Services.Telegram;
using DiscordToTelegramSender.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Telegram.Bot;

// start to build application
var builder = WebApplication.CreateBuilder(args);

// Heroku port
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls("http://*:" + port);
}

// services config
ConfigureSettings(builder);
AddTelegramBot(builder);
AddDiscordBot(builder);

builder.Services.AddHostedService<PipelineRunner>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseRouting();

app.UseEndpoints(configs => configs.MapControllers());

// run application
app.Run();

void ConfigureSettings(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.Configure<DiscordSettings>(webApplicationBuilder.Configuration.GetSection(DiscordSettings.Section));
    webApplicationBuilder.Services.Configure<TelegramSettings>(webApplicationBuilder.Configuration.GetSection(TelegramSettings.Section));
}

void AddTelegramBot(WebApplicationBuilder builder1)
{
    builder1.Services.AddSingleton<ITelegramService, TelegramService>();
    builder1.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(q =>
                                                                          {
                                                                              var settings = q.GetRequiredService<IOptions<TelegramSettings>>()
                                                                                              .Value;
                                                                              return new TelegramBotClient(settings.Token);
                                                                          });
}

void AddDiscordBot(WebApplicationBuilder webApplicationBuilder1)
{
    webApplicationBuilder1.Services.AddSingleton<IDiscordService, DiscordService>();
    webApplicationBuilder1.Services.AddSingleton<DiscordSocketClient>();
    webApplicationBuilder1.Services.AddSingleton<IDiscordCommandsContainer, DiscordCommandsContainer>();
}
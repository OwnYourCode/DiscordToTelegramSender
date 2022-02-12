namespace DiscordToTelegramSender;

public class PingService
{
    private readonly HttpClient _httpClient;

    public PingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> Ping()
    {
        var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!.Split(",")[0];
        return _httpClient.GetAsync(url + "/Ping");
    }
}
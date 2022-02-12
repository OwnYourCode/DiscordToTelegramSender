using Microsoft.AspNetCore.Mvc;

namespace DiscordToTelegramSender.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Ping()
    {
        _logger.LogInformation("Start ping");
        return StatusCode(500);
    }
}
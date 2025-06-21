using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomateDot.Controllers;

[AllowAnonymous]
public class WebhookController : AutomateDotController
{
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger)
    {
        _logger = logger;
    }

    [HttpPost("github")]
    public async Task<IActionResult> Github(dynamic payload)
    {
        _logger.LogInformation("Payload {payload}", (object)payload);
        return Ok();
    }
}
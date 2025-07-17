using AutomateDot.Configurations;
using AutomateDot.Services;
using AutomateDot.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomateDot.Controllers;

[AllowAnonymous]
public class WebhookController : AutomateDotController
{
    private readonly AutomateService _automationService;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger, AutomateService automationService)
    {
        _logger = logger;
        _automationService = automationService;
    }

    [HttpPost("github")]
    public async Task<IActionResult> Github([FromBody] object payload)
    {
        _logger.LogInformation("Github Webhook Payload {payload}", payload);

        var recipes = await _automationService.GetByTriggerId(Constants.Triggers.GITHUB);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<GitHubWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!GitHubWebhookTrigger.ShouldTrigger(this.Request, config!, payload))
                return Ok();

            await _automationService.ExecuteAction(recipe, payload);
        }

        return Ok();
    }

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] object payload)
    {
        _logger.LogInformation("AutomateDot Webhook Payload {payload}", payload);

        var recipes = await _automationService.GetByTriggerId(Constants.Triggers.AUTOMATEDOT);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
                return Ok();

            await _automationService.ExecuteAction(recipe, payload);
        }

        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var recipes = await _automationService.GetByTriggerId(Constants.Triggers.AUTOMATEDOT);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
                return Ok();

            await _automationService.ExecuteAction(recipe);
        }

        return Ok();
    }
}
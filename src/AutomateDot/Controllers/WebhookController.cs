using AutomateDot.Actions;
using AutomateDot.Configurations;
using AutomateDot.Data.Enums;
using AutomateDot.Services;
using AutomateDot.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomateDot.Controllers;

[AllowAnonymous]
public class WebhookController : AutomateDotController
{
    private readonly AutomationService _automationService;
    private readonly ActionsService _actionsService;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger, ActionsService actionsService, AutomationService automationService)
    {
        _logger = logger;
        _actionsService = actionsService;
        _automationService = automationService;
    }

    [HttpPost("github")]
    public async Task<IActionResult> Github([FromBody] dynamic payload)
    {
        _logger.LogInformation("Github Webhook Payload {payload}", (object)payload);

        var recipes = await _automationService.GetByTriggerType(TriggerType.GitHubWebhook);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<GitHubWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!await GitHubWebhookTrigger.ShouldTrigger(this.Request, config!))
                return Ok();

            await _actionsService.Execute(recipe);
        }

        return Ok();
    }

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] dynamic payload)
    {
        _logger.LogInformation("AutomateDot Webhook Payload {payload}", (object)payload);

        var recipes = await _automationService.GetByTriggerType(TriggerType.AutomateDotWebhook);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
                return Ok();

            await _actionsService.Execute(recipe);
        }

        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var recipes = await _automationService.GetByTriggerType(TriggerType.AutomateDotWebhook);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
                return Ok();

            await _actionsService.Execute(recipe);
        }

        return Ok();
    }
}
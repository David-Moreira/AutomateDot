using AutomateDot.Actions;
using AutomateDot.Configurations;
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

        var recipes = await _automationService.GetByTriggerId("a7812c4c-b653-4509-9a47-77a4d89b4652");

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

        var recipes = await _automationService.GetByTriggerId("e1e6e3b0-3d6c-4b31-98a3-468e2c64a0f8");

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
        var recipes = await _automationService.GetByTriggerId("e1e6e3b0-3d6c-4b31-98a3-468e2c64a0f8");

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
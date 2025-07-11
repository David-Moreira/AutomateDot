using AutomateDot.Actions;
using AutomateDot.Configurations;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;
using AutomateDot.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomateDot.Controllers;

[AllowAnonymous]
public class WebhookController : AutomateDotController
{
    private readonly ActionsService _actionsService;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger, ActionsService actionsService)
    {
        _logger = logger;
        this._actionsService = actionsService;
    }

    [HttpPost("github")]
    public async Task<IActionResult> Github([FromBody] dynamic payload)
    {
        _logger.LogInformation("Payload {payload}", (object)payload);

        var recipe = new AutomationRecipe()
        {
            Name = "Main",
            TriggerType = TriggerType.GitHubWebhook,
            TriggerConfiguration = "{ \"TriggerEvent\": \"issues\" }",
            ActionType = ActionType.Gotify,
            ActionConfiguration = "{ \"Url\": \"http://192.168.1.19/message?token=A8L6EauXnHW0-_Y\", \"Message\": \"Hey From AutomateDot!\"}",
        };

        var config = System.Text.Json.JsonSerializer.Deserialize<GitHubWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
        if (!GitHubWebhookTrigger.ShouldTrigger(this.Request, config!))
            return Ok();

        await _actionsService.Execute(recipe.ActionType, recipe.ActionConfiguration);

        return Ok();
    }

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] dynamic payload)
    {
        _logger.LogInformation("Payload {payload}", (object)payload);

        var recipe = new AutomationRecipe()
        {
            Name = "Main",
            TriggerType = TriggerType.GitHubWebhook,
            TriggerConfiguration = "{ \"TriggerEvent\": \"issues\" }",
            ActionType = ActionType.Gotify,
            ActionConfiguration = "{ \"Url\": \"http://192.168.1.19/message?token=A8L6EauXnHW0-_Y\", \"Message\": \"Hey From AutomateDot!\"}",
        };

        var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
        if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
            return Ok();

        await _actionsService.Execute(recipe.ActionType, recipe.ActionConfiguration);

        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var recipe = new AutomationRecipe()
        {
            Name = "Main",
            TriggerType = TriggerType.GitHubWebhook,
            TriggerConfiguration = "{ \"TriggerEvent\": \"issues\" }",
            ActionType = ActionType.Gotify,
            ActionConfiguration = "{ \"Url\": \"http://192.168.1.19/message?token=A8L6EauXnHW0-_Y\", \"Message\": \"Hey From AutomateDot!\"}",
        };

        var config = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
        if (!AutomateDotWebhookTrigger.ShouldTrigger(this.Request, config!))
            return Ok();

        await _actionsService.Execute(recipe.ActionType, recipe.ActionConfiguration);

        return Ok();
    }
}
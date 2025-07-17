using AutomateDot.Configurations;
using AutomateDot.Services;
using AutomateDot.Triggers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json;

namespace AutomateDot.Controllers;

[AllowAnonymous]
public class WebhookController : AutomateDotController
{
    private readonly AutomateService _automationService;
    private readonly GitHubWebhookTrigger _gitHubWebhookTrigger;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger, AutomateService automationService, GitHubWebhookTrigger gitHubWebhookTrigger)
    {
        _logger = logger;
        _automationService = automationService;
        _gitHubWebhookTrigger = gitHubWebhookTrigger;
    }

    [HttpPost("github")]
    public async Task<IActionResult> Github()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
        var payload = await reader.ReadToEndAsync();

        _logger.LogInformation("Github Webhook Payload {payload}", payload);

        var recipes = await _automationService.GetByTriggerId(Constants.Triggers.GITHUB);

        foreach (var recipe in recipes)
        {
            var config = System.Text.Json.JsonSerializer.Deserialize<GitHubWebhookTriggerConfiguration>(recipe.TriggerConfiguration);
            if (!_gitHubWebhookTrigger.ShouldTrigger(this.Request, config!, payload))
            {
                _logger.LogInformation("Automate {recipe} conditions failed for this github request.", recipe.Name);
                return Ok();
            }

            var payloadAsObject = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(payload);
            await _automationService.ExecuteAction(recipe, payloadAsObject);
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
            {
                _logger.LogInformation("Automate {recipe} conditions failed for this automatedot request.", recipe.Name);
                return Ok();
            }

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
            {
                _logger.LogInformation("Automate {recipe} conditions failed for this automatedot request.", recipe.Name);
                return Ok();
            }

            await _automationService.ExecuteAction(recipe);
        }

        return Ok();
    }
}
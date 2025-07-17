using AutomateDot.Configurations;

using System.Text;
using System.Text.Json;

namespace AutomateDot.Actions;

public sealed class SlackWebhookAction(IHttpClientFactory httpClientFactory) : IActionHandler<SlackWebhookConfiguration>
{
    public async Task ExecuteAsync(SlackWebhookConfiguration configuration, object? triggerPayload = null)
    {
        var url = ActionHelper.ReplacePlaceholders(configuration.Url, triggerPayload);
        var message = ActionHelper.ReplacePlaceholders(configuration.Message, triggerPayload);

        using var client = httpClientFactory.CreateClient();
        var payload = JsonSerializer.Serialize(new { text = message });
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
}
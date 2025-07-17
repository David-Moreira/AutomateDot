using AutomateDot.Configurations;

using System.Text;
using System.Text.Json;

namespace AutomateDot.Actions;

public sealed class DiscordWebhookAction(IHttpClientFactory httpClientFactory) : IActionHandler<DiscordWebhookConfiguration>
{
    public async Task ExecuteAsync(DiscordWebhookConfiguration configuration, object? triggerPayload = null)
    {
        var url = ActionHelper.ReplacePlaceholders(configuration.Url, triggerPayload);
        var content = ActionHelper.ReplacePlaceholders(configuration.Message, triggerPayload);

        using var client = httpClientFactory.CreateClient();
        var payload = JsonSerializer.Serialize(new { content });
        using var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, httpContent);
        response.EnsureSuccessStatusCode();
    }
}
using AutomateDot.Configurations;

using System.Text;

namespace AutomateDot.Actions;

public sealed class SendWebhookAction(IHttpClientFactory HttpClientFactory) : IActionHandler<SendWebhookConfiguration>
{
    public async Task ExecuteAsync(SendWebhookConfiguration configuration, object? triggerPayload = null)
    {
        using var client = HttpClientFactory.CreateClient();
        var content = new StringContent(ActionHelper.ReplacePlaceholders(configuration.Message, triggerPayload), Encoding.UTF8, "application/json");
        await client.PostAsync(configuration.Url, content);
    }
}
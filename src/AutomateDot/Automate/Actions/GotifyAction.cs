using AutomateDot.Configurations;

using System.Text;
using System.Text.Json;

namespace AutomateDot.Actions;

public sealed class GotifyAction(IHttpClientFactory HttpClientFactory) : IActionHandler<GotifyConfiguration>
{
    public async Task ExecuteAsync(GotifyConfiguration configuration, object? triggerPayload = null)
    {
        using var client = HttpClientFactory.CreateClient();

        var payload = new
        {
            title = ActionHelper.ReplacePlaceholders(configuration.Title, triggerPayload),
            message = ActionHelper.ReplacePlaceholders(configuration.Message, triggerPayload),
            priority = configuration.Priority
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await client.PostAsync(configuration.Url, content);
    }
}
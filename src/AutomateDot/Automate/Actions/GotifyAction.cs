using AutomateDot.Configurations;

using System.Text;
using System.Text.Json;

namespace AutomateDot.Actions;

public sealed class GotifyAction(IHttpClientFactory HttpClientFactory)
{
    public async Task ExecuteAsync(GotifyConfiguration configuration)
    {
        using var client = HttpClientFactory.CreateClient();

        var payload = new
        {
            title = configuration.Title,
            message = configuration.Message,
            priority = configuration.Priority
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await client.PostAsync(configuration.Url, content);
    }
}
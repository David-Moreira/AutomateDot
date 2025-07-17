using AutomateDot.Configurations;

namespace AutomateDot.Triggers;

public class PingTrigger(IHttpClientFactory HttpClientFactory, ILogger<PingTrigger> Logger)
{
    public async Task<bool> ShouldTrigger(PingTriggerConfiguration configuration)
    {
        using var client = HttpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync(configuration.Url);
            return (configuration.IsSuccess && response.IsSuccessStatusCode) || (!configuration.IsSuccess && !response.IsSuccessStatusCode);
        }
        catch (Exception)
        {
            Logger.LogError("Ping failed for Url: {Url}", configuration.Url);
            return !configuration.IsSuccess;
        }
    }
}
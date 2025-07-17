using AutomateDot.Configurations;

namespace AutomateDot.Triggers;

public static class AutomateDotWebhookTrigger
{
    public static bool ShouldTrigger(HttpRequest request, AutomateDotWebhookTriggerConfiguration configuration)
    {
        var eventType = request.Headers["X-AutomateDot-Event"].ToString();
        return string.Equals(eventType, configuration.TriggerEvent, StringComparison.OrdinalIgnoreCase);
    }
}

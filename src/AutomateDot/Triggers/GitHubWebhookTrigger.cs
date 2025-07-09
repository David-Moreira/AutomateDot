using AutomateDot.Configurations;

namespace AutomateDot.Triggers;

public static class GitHubWebhookTrigger
{
    public static bool ShouldTrigger(HttpRequest request, GitHubWebhookTriggerConfiguration configuration)
    {
        var eventType = request.Headers["X-GitHub-Event"];
        return eventType == configuration.TriggerEvent;
    }
}
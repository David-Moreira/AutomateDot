namespace AutomateDot.Configurations;

public sealed class GitHubWebhookTriggerConfiguration()
{
    public string TriggerEvent { get; set; } = string.Empty;
}
namespace AutomateDot.Data.Enums;

public enum Trigger
{
    GitHubWebhook,
    AutomateDotWebhook
}

public static class TriggerTypeExtensions
{
    public static string ToFriendlyString(this Trigger type)
    {
        return type switch
        {
            Trigger.GitHubWebhook => "GitHub Webhook",
            Trigger.AutomateDotWebhook => "AutomateDot Webhook",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
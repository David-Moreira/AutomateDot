namespace AutomateDot.Data.Enums;

public enum TriggerType
{
    GitHubWebhook,
    AutomateDotWebhook
}

public static class TriggerTypeExtensions
{
    public static string ToFriendlyString(this TriggerType type)
    {
        return type switch
        {
            TriggerType.GitHubWebhook => "GitHub Webhook",
            TriggerType.AutomateDotWebhook => "AutomateDot Webhook",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
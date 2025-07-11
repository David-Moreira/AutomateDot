namespace AutomateDot.Data.Enums;

public enum ActionType
{
    SendWebhook,
    Gotify,
    Script
}

public static class ActionTypeExtensions
{
    public static string ToFriendlyString(this ActionType type)
    {
        return type switch
        {
            ActionType.SendWebhook => "Send Webhook",
            ActionType.Gotify => "Gotify",
            ActionType.Script => "Script",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
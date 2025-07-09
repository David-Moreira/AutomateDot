namespace AutomateDot.Data.Enums;

public enum ActionType
{
    SendWebhook,
    Gotify
}

public static class ActionTypeExtensions
{
    public static string ToFriendlyString(this ActionType type)
    {
        return type switch
        {
            ActionType.SendWebhook => "Send Webhook",
            ActionType.Gotify => "Gotify",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
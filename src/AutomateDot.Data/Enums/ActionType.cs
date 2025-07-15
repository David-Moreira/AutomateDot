namespace AutomateDot.Data.Enums;

public enum Action
{
    SendWebhook,
    Gotify,
    Script
}

public static class ActionTypeExtensions
{
    public static string ToFriendlyString(this Action type)
    {
        return type switch
        {
            Action.SendWebhook => "Send Webhook",
            Action.Gotify => "Gotify",
            Action.Script => "Script",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
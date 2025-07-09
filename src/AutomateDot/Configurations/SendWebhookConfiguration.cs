namespace AutomateDot.Configurations;

public sealed class SendWebhookConfiguration()
{
    public string Url { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

namespace AutomateDot.Data.Enums;

/// <summary>
/// https://docs.github.com/en/webhooks/webhook-events-and-payloads
/// </summary>
public enum GitHubWebhookTriggerEvent
{
    CreateBranch,
    DeleteBranch,
    CreateTag,
    DeleteTag,
    Released,
    PreReleased,
    NewIssue,
    NewPullRequest,
}

public static class GitHubWebhookTriggerEventExtensions
{
    public static string ToFriendlyString(this GitHubWebhookTriggerEvent type)
    {
        return type switch
        {
            GitHubWebhookTriggerEvent.CreateBranch => "Create Branch",
            GitHubWebhookTriggerEvent.DeleteBranch => "Delete Branch",
            GitHubWebhookTriggerEvent.CreateTag => "Create Tag",
            GitHubWebhookTriggerEvent.DeleteTag => "Delete Tag",
            GitHubWebhookTriggerEvent.Released => "Released",
            GitHubWebhookTriggerEvent.PreReleased => "Pre-release",
            GitHubWebhookTriggerEvent.NewIssue => "New Issue",
            GitHubWebhookTriggerEvent.NewPullRequest => "New Pull Request",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
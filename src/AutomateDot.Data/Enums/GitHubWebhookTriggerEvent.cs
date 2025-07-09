namespace AutomateDot.Data.Enums;

/// <summary>
/// https://docs.github.com/en/webhooks/webhook-events-and-payloads
/// </summary>
public enum GitHubWebhookTriggerEvent
{
    Create,
    Delete,
    Deployment,
    Discussion,
    Issues,
    PullRequest,
    WorkflowDispatch,
    WorkflowJob,
    WorkflowRun
}

public static class GitHubWebhookTriggerEventExtensions
{
    public static string ToFriendlyString(this GitHubWebhookTriggerEvent type)
    {
        return type switch
        {
            GitHubWebhookTriggerEvent.Create => "Create",
            GitHubWebhookTriggerEvent.Delete => "Delete",
            GitHubWebhookTriggerEvent.Deployment => "Deployment",
            GitHubWebhookTriggerEvent.Discussion => "Discussion",
            GitHubWebhookTriggerEvent.Issues => "Issues",
            GitHubWebhookTriggerEvent.PullRequest => "Pull Request",
            GitHubWebhookTriggerEvent.WorkflowDispatch => "Workflow Dispatch",
            GitHubWebhookTriggerEvent.WorkflowJob => "Workflow Job",
            GitHubWebhookTriggerEvent.WorkflowRun => "Workflow Run",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
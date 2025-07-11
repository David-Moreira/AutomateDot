using AutomateDot.Data.Enums;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class GitHubWebhookTriggerConfiguration()
{
    [Required]
    public string Repository { get; set; } = string.Empty;

    public string GithubTriggerEvent
        => TriggerEvent switch
        {
            GitHubWebhookTriggerEvent.CreateBranch => "create",
            GitHubWebhookTriggerEvent.DeleteBranch => "delete",
            GitHubWebhookTriggerEvent.CreateTag => "create",
            GitHubWebhookTriggerEvent.DeleteTag => "delete",
            GitHubWebhookTriggerEvent.NewPullRequest => "pull_request",
            GitHubWebhookTriggerEvent.NewIssue => "issues",
            _ => TriggerEvent.ToString().ToLower()
        };

    public GitHubWebhookTriggerEvent TriggerEvent { get; set; }
}
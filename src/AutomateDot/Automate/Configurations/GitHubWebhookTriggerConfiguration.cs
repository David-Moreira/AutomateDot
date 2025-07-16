using AutomateDot.Components.Automation.Triggers;
using AutomateDot.Data.Enums;
using AutomateDot.Payloads;
using AutomateDot.Payloads.GitHub;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class GitHubWebhookTriggerConfiguration : ITriggerConfiguration
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

    public Dictionary<string, string> GetPayloadFields()
    {
        switch (TriggerEvent)
        {
            case GitHubWebhookTriggerEvent.CreateBranch:
                return PayloadFieldHelper.GetFlattenedFields<GithubCreateEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.DeleteBranch:
                return PayloadFieldHelper.GetFlattenedFields<GithubDeleteEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.CreateTag:
                return PayloadFieldHelper.GetFlattenedFields<GithubCreateEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.DeleteTag:
                return PayloadFieldHelper.GetFlattenedFields<GithubDeleteEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.Released:
                return PayloadFieldHelper.GetFlattenedFields<GithubReleaseEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.NewIssue:
                return PayloadFieldHelper.GetFlattenedFields<GithubIssuesEventPayload>().ToDictionary(x => x, x => x);

            case GitHubWebhookTriggerEvent.NewPullRequest:
                return PayloadFieldHelper.GetFlattenedFields<GithubPullRequestEventPayload>().ToDictionary(x => x, x => x);

            default:
                break;
        }

        return new();
    }
}
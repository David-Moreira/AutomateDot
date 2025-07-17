using AutomateDot.Components.Automate.Triggers;
using AutomateDot.Data.Enums;
using AutomateDot.Payloads;
using AutomateDot.Payloads.GitHub;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class GitHubWebhookTriggerConfiguration : ITriggerConfiguration
{
    [Required]
    public string Repository { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public string GithubTriggerEvent
        => TriggerEvent switch
        {
            GitHubWebhookTriggerEvent.CreateBranch => "create",
            GitHubWebhookTriggerEvent.DeleteBranch => "delete",
            GitHubWebhookTriggerEvent.CreateTag => "create",
            GitHubWebhookTriggerEvent.DeleteTag => "delete",
            GitHubWebhookTriggerEvent.NewPullRequest => "pull_request",
            GitHubWebhookTriggerEvent.NewIssue => "issues",
            GitHubWebhookTriggerEvent.Released => "release",
            GitHubWebhookTriggerEvent.PreReleased => "release",
            _ => TriggerEvent.ToString().ToLower()
        };

    public GitHubWebhookTriggerEvent TriggerEvent { get; set; }

    public Dictionary<string, string> GetPayloadFields()
    {
        Dictionary<string, string> payloadFields = [];

        switch (TriggerEvent)
        {
            case GitHubWebhookTriggerEvent.CreateBranch:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubCreateEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.DeleteBranch:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubDeleteEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.CreateTag:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubCreateEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.DeleteTag:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubDeleteEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.Released:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubReleaseEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.PreReleased:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubReleaseEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.NewIssue:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubIssuesEventPayload>().ToDictionary(x => x, x => x);
                break;

            case GitHubWebhookTriggerEvent.NewPullRequest:
                payloadFields = PayloadFieldHelper.GetFlattenedFields<GithubPullRequestEventPayload>().ToDictionary(x => x, x => x);
                break;

            default:
                break;
        }

        return PayloadFieldHelper.GetWithAdditionalPayloadMappingOptions(payloadFields);
    }
}
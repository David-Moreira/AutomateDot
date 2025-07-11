using AutomateDot.Data.Enums;

namespace AutomateDot.Configurations;

public sealed class GitHubWebhookTriggerConfiguration()
{
    public string GithubTriggerEvent
        => TriggerEvent switch
        {
            GitHubWebhookTriggerEvent.PullRequest => "pull_request",
            GitHubWebhookTriggerEvent.WorkflowDispatch => "workflow_dispatch",
            GitHubWebhookTriggerEvent.WorkflowJob => "workflow_job",
            GitHubWebhookTriggerEvent.WorkflowRun => "workflow_run",
            _ => TriggerEvent.ToString().ToLower()
        };

    public GitHubWebhookTriggerEvent TriggerEvent { get; set; }
}
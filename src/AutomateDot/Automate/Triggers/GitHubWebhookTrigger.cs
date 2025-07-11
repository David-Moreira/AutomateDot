using AutomateDot.Configurations;
using AutomateDot.Payloads.GitHub;

namespace AutomateDot.Triggers;

public static class GitHubWebhookTrigger
{
    public static async Task<bool> ShouldTrigger(HttpRequest request, GitHubWebhookTriggerConfiguration configuration)
    {
        var eventType = request.Headers["X-GitHub-Event"].ToString();
        var isEvent = string.Equals(eventType, configuration.GithubTriggerEvent, StringComparison.OrdinalIgnoreCase);

        if (isEvent)
        {
            switch (configuration.TriggerEvent)
            {
                case Data.Enums.GitHubWebhookTriggerEvent.CreateBranch:
                    var createEventPayload = await request.ReadFromJsonAsync<GithubCreateEventPayload>();
                    return createEventPayload?.RefType == "branch";

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteBranch:
                    var deleteEventPayload = await request.ReadFromJsonAsync<GithubDeleteEventPayload>();
                    return deleteEventPayload?.RefType == "branch";

                case Data.Enums.GitHubWebhookTriggerEvent.CreateTag:
                    var createTagEventPayload = await request.ReadFromJsonAsync<GithubCreateEventPayload>();
                    return createTagEventPayload?.RefType == "tag";

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteTag:
                    var deleteTagEventPayload = await request.ReadFromJsonAsync<GithubDeleteEventPayload>();
                    return deleteTagEventPayload?.RefType == "tag";

                case Data.Enums.GitHubWebhookTriggerEvent.Released:
                    var releaseEventPayload = await request.ReadFromJsonAsync<GithubReleaseEventPayload>();
                    return releaseEventPayload?.Action == "released";

                case Data.Enums.GitHubWebhookTriggerEvent.NewIssue:
                    var issuesEventPayload = await request.ReadFromJsonAsync<GithubIssuesEventPayload>();
                    return issuesEventPayload?.Action == "opened";

                case Data.Enums.GitHubWebhookTriggerEvent.NewPullRequest:
                    var pullRequestEventPayload = await request.ReadFromJsonAsync<GithubPullRequestEventPayload>();
                    return pullRequestEventPayload?.Action == "opened";

                default:
                    break;
            }
        }

        return isEvent;
    }
}
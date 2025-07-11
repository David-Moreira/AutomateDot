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

                    return createEventPayload is not null &&
                    string.Equals(createEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(createEventPayload.RefType, "branch", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteBranch:
                    var deleteEventPayload = await request.ReadFromJsonAsync<GithubDeleteEventPayload>();
                    return deleteEventPayload is not null &&
                    string.Equals(deleteEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(deleteEventPayload?.RefType, "branch", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.CreateTag:
                    var createTagEventPayload = await request.ReadFromJsonAsync<GithubCreateEventPayload>();
                    return createTagEventPayload is not null &&
                    string.Equals(createTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(createTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteTag:
                    var deleteTagEventPayload = await request.ReadFromJsonAsync<GithubDeleteEventPayload>();
                    return deleteTagEventPayload is not null &&
                    string.Equals(deleteTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(deleteTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.Released:
                    var releaseEventPayload = await request.ReadFromJsonAsync<GithubReleaseEventPayload>();
                    return releaseEventPayload is not null &&
                    string.Equals(releaseEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(releaseEventPayload?.Action, "released", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.NewIssue:
                    var issuesEventPayload = await request.ReadFromJsonAsync<GithubIssuesEventPayload>();
                    return issuesEventPayload is not null &&
                    string.Equals(issuesEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(issuesEventPayload?.Action, "opened", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.NewPullRequest:
                    var pullRequestEventPayload = await request.ReadFromJsonAsync<GithubPullRequestEventPayload>();
                    return pullRequestEventPayload is not null &&
                    string.Equals(pullRequestEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(pullRequestEventPayload?.Action, "opened", StringComparison.OrdinalIgnoreCase);

                default:
                    break;
            }
        }

        return false;
    }
}
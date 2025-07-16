using AutomateDot.Configurations;
using AutomateDot.Payloads.GitHub;

namespace AutomateDot.Triggers;

public static class GitHubWebhookTrigger
{
    public static bool ShouldTrigger(HttpRequest request, GitHubWebhookTriggerConfiguration configuration, dynamic payload)
    {
        string payloadAsJson = System.Text.Json.JsonSerializer.Serialize(payload);

        var eventType = request.Headers["X-GitHub-Event"].ToString();
        var isEvent = string.Equals(eventType, configuration.GithubTriggerEvent, StringComparison.OrdinalIgnoreCase);

        if (isEvent)
        {
            switch (configuration.TriggerEvent)
            {
                case Data.Enums.GitHubWebhookTriggerEvent.CreateBranch:
                    var createEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubCreateEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);

                    return createEventPayload is not null &&
                    string.Equals(createEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(createEventPayload.RefType, "branch", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteBranch:
                    var deleteEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubDeleteEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
                    return deleteEventPayload is not null &&
                    string.Equals(deleteEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(deleteEventPayload?.RefType, "branch", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.CreateTag:
                    var createTagEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubCreateEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
                    return createTagEventPayload is not null &&
                    string.Equals(createTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(createTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.DeleteTag:
                    var deleteTagEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubDeleteEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
                    return deleteTagEventPayload is not null &&
                    string.Equals(deleteTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(deleteTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.Released:
                    var releaseEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubReleaseEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
                    return releaseEventPayload is not null &&
                    string.Equals(releaseEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(releaseEventPayload?.Action, "released", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.NewIssue:
                    var issuesEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubIssuesEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
                    return issuesEventPayload is not null &&
                    string.Equals(issuesEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(issuesEventPayload?.Action, "opened", StringComparison.OrdinalIgnoreCase);

                case Data.Enums.GitHubWebhookTriggerEvent.NewPullRequest:
                    var pullRequestEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubPullRequestEventPayload>(payloadAsJson, JsonSerializerDefaults.Options);
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
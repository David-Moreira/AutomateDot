using AutomateDot.Configurations;
using AutomateDot.Payloads.GitHub;
using AutomateDot.Utilities;

using System.Security.Cryptography;
using System.Text;

namespace AutomateDot.Triggers;

public class GitHubWebhookTrigger(ILogger<GitHubWebhookTrigger> Logger)
{
    public bool ShouldTrigger(HttpRequest request, GitHubWebhookTriggerConfiguration configuration, string payloadAsString)
    {
        if (!IsValidGitHubSignature(request, configuration.Secret, payloadAsString))
        {
            Logger.LogWarning("GitHubWebhookTrigger failed secret validation");
            return false;
        }

        var eventType = request.Headers["X-GitHub-Event"].ToString();
        var isEvent = string.Equals(eventType, configuration.GithubTriggerEvent, StringComparison.OrdinalIgnoreCase);

        if (!isEvent)
        {
            Logger.LogInformation("GitHubWebhookTrigger event {event} is not a match", eventType);
            return false;
        }

        Logger.LogInformation("GitHubWebhookTrigger event {event} is a match, evaluating whether it matches specific trigger conditions for {triggerEvent}...", eventType, configuration.TriggerEvent);

        switch (configuration.TriggerEvent)
        {
            case Data.Enums.GitHubWebhookTriggerEvent.CreateBranch:
                var createEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubCreateEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);

                return createEventPayload is not null &&
                string.Equals(createEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(createEventPayload.RefType, "branch", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.DeleteBranch:
                var deleteEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubDeleteEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return deleteEventPayload is not null &&
                string.Equals(deleteEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(deleteEventPayload?.RefType, "branch", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.CreateTag:
                var createTagEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubCreateEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return createTagEventPayload is not null &&
                string.Equals(createTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(createTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.DeleteTag:
                var deleteTagEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubDeleteEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return deleteTagEventPayload is not null &&
                string.Equals(deleteTagEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(deleteTagEventPayload?.RefType, "tag", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.Released:
                var releaseEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubReleaseEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return releaseEventPayload is not null &&
                string.Equals(releaseEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(releaseEventPayload?.Action, "released", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.PreReleased:
                var preReleaseEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubReleaseEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return preReleaseEventPayload is not null &&
                string.Equals(preReleaseEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(preReleaseEventPayload?.Action, "prereleased", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.NewIssue:
                var issuesEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubIssuesEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return issuesEventPayload is not null &&
                string.Equals(issuesEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(issuesEventPayload?.Action, "opened", StringComparison.OrdinalIgnoreCase);

            case Data.Enums.GitHubWebhookTriggerEvent.NewPullRequest:
                var pullRequestEventPayload = System.Text.Json.JsonSerializer.Deserialize<GithubPullRequestEventPayload>(payloadAsString, JsonSerializerConfiguration.DefaultOptions);
                return pullRequestEventPayload is not null &&
                string.Equals(pullRequestEventPayload.Repository.Name, configuration.Repository, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(pullRequestEventPayload?.Action, "opened", StringComparison.OrdinalIgnoreCase);

            default:
                break;
        }

        return false;
    }

    private static bool IsValidGitHubSignature(HttpRequest request, string secret, string payloadAsString)
    {
        if (string.IsNullOrEmpty(secret))
            return true;

        if (!request.Headers.TryGetValue("X-Hub-Signature-256", out var signatureHeader))
            return false;

        var signature = signatureHeader.ToString();
        if (!signature.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase))
            return false;

        var expectedSignature = signature.Substring("sha256=".Length);

        var bodyBytes = Encoding.UTF8.GetBytes(payloadAsString);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hashBytes = hmac.ComputeHash(bodyBytes);
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(hashString),
            Encoding.UTF8.GetBytes(expectedSignature)
        );
    }
}
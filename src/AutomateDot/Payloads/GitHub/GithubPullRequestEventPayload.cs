namespace AutomateDot.Payloads.GitHub;

public class GithubPullRequestEventPayload
{
    public string Action { get; set; }               // e.g., "opened", "closed", "synchronize", etc.
    public GithubPullRequest PullRequest { get; set; }
    public GithubRepository Repository { get; set; }
    public GithubUser Sender { get; set; }
}

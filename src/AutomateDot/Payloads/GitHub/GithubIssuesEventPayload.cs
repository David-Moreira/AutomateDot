#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubIssuesEventPayload
{
    public string Action { get; set; }         // e.g., "opened", "closed", "labeled", etc.
    public GithubIssue Issue { get; set; }
    public GithubRepository Repository { get; set; }
    public GithubUser Sender { get; set; }
}
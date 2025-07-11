namespace AutomateDot.Payloads.GitHub;

public class GithubReleaseEventPayload
{
    public string Action { get; set; }       // e.g., "published", "edited", "deleted", etc.
    public GithubRelease Release { get; set; }
    public GithubRepository Repository { get; set; }
    public GithubUser Sender { get; set; }
}

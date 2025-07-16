#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubPullRequest
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string State { get; set; }                // "open" or "closed"
    public string Title { get; set; }
    public GithubUser User { get; set; }
    public string Body { get; set; }
    public bool Draft { get; set; }
    public GithubBranchRef Head { get; set; }
    public GithubBranchRef Base { get; set; }
    public bool Merged { get; set; }
    public int Commits { get; set; }
    public int Additions { get; set; }
    public int Deletions { get; set; }
    public int ChangedFiles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? MergedAt { get; set; }
    public string HtmlUrl { get; set; }
}
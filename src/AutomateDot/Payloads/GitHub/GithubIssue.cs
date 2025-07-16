#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubIssue
{
    public long Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public GithubUser User { get; set; }
    public List<GithubLabel> Labels { get; set; }
    public string State { get; set; }          // "open" or "closed"
    public bool Locked { get; set; }
    public int Comments { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string HtmlUrl { get; set; }
}
#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubRelease
{
    public long Id { get; set; }
    public string TagName { get; set; }
    public string TargetCommitish { get; set; }
    public string Name { get; set; }
    public bool Draft { get; set; }
    public bool Prerelease { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public GithubUser Author { get; set; }
    public string HtmlUrl { get; set; }
    public string TarballUrl { get; set; }
    public string ZipballUrl { get; set; }
    public string Body { get; set; }
}
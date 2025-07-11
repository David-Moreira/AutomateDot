namespace AutomateDot.Payloads.GitHub;

public class GithubUser
{
    public string Login { get; set; }
    public long Id { get; set; }
    public string AvatarUrl { get; set; }
    public string HtmlUrl { get; set; }
    public string Type { get; set; }
    public bool SiteAdmin { get; set; }
}
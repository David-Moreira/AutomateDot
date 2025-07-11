namespace AutomateDot.Payloads.GitHub;

public class GithubRepositoryRepoInfo
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public GithubUser Owner { get; set; }
    public string HtmlUrl { get; set; }
}

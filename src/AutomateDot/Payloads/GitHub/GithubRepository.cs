namespace AutomateDot.Payloads.GitHub;

public class GithubRepository
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public GithubUser Owner { get; set; }
    public bool Private { get; set; }
    public string HtmlUrl { get; set; }
    public string Description { get; set; }
    public bool Fork { get; set; }
    public string Url { get; set; }
    public int StargazersCount { get; set; }
    public int WatchersCount { get; set; }
    public string DefaultBranch { get; set; }
}

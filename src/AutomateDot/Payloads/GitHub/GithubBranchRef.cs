#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubBranchRef
{
    public string Label { get; set; }
    public string Ref { get; set; }
    public string Sha { get; set; }
    public GithubRepositoryRepoInfo Repo { get; set; }
    public GithubUser User { get; set; }
}
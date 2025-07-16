#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubCreateEventPayload
{
    public string Ref { get; set; }         // The name of the created branch or tag
    public string RefType { get; set; }     // Either "branch" or "tag"
    public string MasterBranch { get; set; } // The default branch of the repository (usually "main" or "master")
    public string Description { get; set; }  // The repository description
    public GithubRepository Repository { get; set; }
    public GithubUser Sender { get; set; }
    public GithubUser Owner { get; set; }
}
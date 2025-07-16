#nullable disable

namespace AutomateDot.Payloads.GitHub;

public class GithubDeleteEventPayload
{
    public string Ref { get; set; }         // The name of the deleted branch or tag
    public string RefType { get; set; }     // Either "branch" or "tag"
    public string PusherType { get; set; }  // Usually "user"
    public GithubRepository Repository { get; set; }
    public GithubUser Sender { get; set; }
}
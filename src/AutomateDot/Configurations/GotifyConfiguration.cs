namespace AutomateDot.Configurations;

public sealed class GotifyConfiguration()
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Priority { get; set; }
}
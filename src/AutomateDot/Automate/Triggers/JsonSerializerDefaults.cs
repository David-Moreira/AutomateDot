namespace AutomateDot.Triggers;

public static class JsonSerializerDefaults
{
    public static readonly System.Text.Json.JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}

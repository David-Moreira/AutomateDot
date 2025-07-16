namespace AutomateDot.Utilities;

public static class JsonSerializerConfiguration
{
    public static readonly System.Text.Json.JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static readonly System.Text.Json.JsonSerializerOptions PrettyWriteOptions = new System.Text.Json.JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };
}
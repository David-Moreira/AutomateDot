using AutomateDot.Payloads;
using AutomateDot.Utilities;

using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AutomateDot.Actions;

public static partial class ActionHelper
{
    public static string ReplacePlaceholders(string template, object? payload)
    {
        if (payload is null)
            return template;

        var result = PayloadFieldPlaceholderRegex().Replace(template, match =>
        {
            object? value = payload;
            var placeholder = match.Groups[1].Value;

            if (placeholder == PayloadFieldHelper.PAYLOAD_FULL)
            {
                return JsonSerializer.Serialize(value, JsonSerializerConfiguration.PrettyWriteOptions);
            }

            var path = placeholder.Split('.');

            foreach (var prop in path)
            {
                if (value == null)
                    break;

                if (value is JsonElement jsonElement)
                {
                    value = GetJsonElementPropertyCaseInsensitive(jsonElement, prop);
                }
                else
                {
                    var type = value.GetType();
                    var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => string.Equals(p.Name, prop, StringComparison.OrdinalIgnoreCase));
                    value = property?.GetValue(value);
                }
            }

            if (value is JsonElement finalElement)
            {
                value = finalElement.ValueKind switch
                {
                    JsonValueKind.String => finalElement.GetString(),
                    JsonValueKind.Number => finalElement.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    JsonValueKind.Null => null,
                    _ => finalElement.GetRawText()
                };
            }

            return value?.ToString() ?? string.Empty;
        });

        result = PayloadFieldEmptyPlaceholderRegex().Replace(result, match =>
        {
            return string.Empty;
        });
        return result;
    }

    private static object? GetJsonElementPropertyCaseInsensitive(JsonElement element, string property)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var prop in element.EnumerateObject())
            {
                if (string.Equals(prop.Name, property, StringComparison.OrdinalIgnoreCase))
                    return prop.Value;
            }
        }
        return null;
    }

    [GeneratedRegex(@"\{\{\s*([\w\.]+)\s*\}\}")]
    public static partial Regex PayloadFieldPlaceholderRegex();

    [GeneratedRegex(@"\{\{([\s]+)\}\}")]
    public static partial Regex PayloadFieldEmptyPlaceholderRegex();
}
using System.Reflection;

namespace AutomateDot.Payloads;

public static class PayloadFieldHelper
{
    public static List<string> GetFlattenedFields<T>(int maxDepth = 3)
    {
        return GetFlattenedFields(typeof(T), maxDepth);
    }

    public static List<string> GetFlattenedFields(Type type, int maxDepth = 3)
    {
        var result = new List<string>();
        GetFlattenedFieldsRecursive(type, null, result, new HashSet<Type>(), maxDepth);
        return result;
    }

    private static void GetFlattenedFieldsRecursive(
        Type type,
        string? prefix,
        List<string> result,
        HashSet<Type> visited,
        int depth)
    {
        if (depth <= 0 || type == typeof(string) || type.IsPrimitive || type.IsEnum)
            return;

        if (!visited.Add(type))
            return; // Prevent cycles

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propType = prop.PropertyType;
            var fieldName = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

            if (propType == typeof(string) || propType.IsPrimitive || propType.IsEnum)
            {
                result.Add(fieldName);
            }
            else
            {
                // Add the property itself if it's a leaf (e.g., bool, long, etc.)
                if (propType.Namespace != null && propType.Namespace.StartsWith("System") && !typeof(IEnumerable<object>).IsAssignableFrom(propType))
                {
                    result.Add(fieldName);
                }
                else
                {
                    // Recurse into complex types
                    GetFlattenedFieldsRecursive(propType, fieldName, result, visited, depth - 1);
                }
            }
        }
        visited.Remove(type);
    }
}
using System.Diagnostics.CodeAnalysis;

namespace AutomateDot.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
    {
        if (enumerable != null)
        {
            return !enumerable.Any();
        }

        return true;
    }
}
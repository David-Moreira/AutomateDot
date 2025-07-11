using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class ScriptConfiguration()
{
    [Required]
    public string File { get; set; } = string.Empty;

    public string Arguments { get; set; } = string.Empty;
}
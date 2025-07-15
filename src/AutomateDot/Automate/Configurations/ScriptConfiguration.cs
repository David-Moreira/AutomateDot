using AutomateDot.Components.Automation;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class ScriptConfiguration : IActionConfiguration
{
    [Required]
    public string File { get; set; } = string.Empty;

    public string Arguments { get; set; } = string.Empty;
}
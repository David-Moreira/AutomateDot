using AutomateDot.Components.Automation.Triggers;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class PingTriggerConfiguration : ITriggerConfiguration
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Minutes { get; set; } = 5;

    public bool IsSuccess { get; set; } = true;
}
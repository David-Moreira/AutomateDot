using AutomateDot.Components.Automation;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class CronTriggerConfiguration : ITriggerConfiguration
{
    [Required]
    public string CronExpression { get; set; } = string.Empty;

    public string? Description { get; set; }
}
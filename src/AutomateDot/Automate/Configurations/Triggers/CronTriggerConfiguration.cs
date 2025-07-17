using AutomateDot.Components.Automate.Triggers;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class CronTriggerConfiguration : ITriggerConfiguration
{
    [Required]
    [CronExpression]
    public string CronExpression { get; set; } = string.Empty;

    public string? Description { get; set; }
}
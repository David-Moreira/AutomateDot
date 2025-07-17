using AutomateDot.Components.Automation.Actions;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class SmtpEmailConfiguration : IActionConfiguration
{
    [Required]
    public string SmtpHost { get; set; } = string.Empty;

    [Required]
    public int SmtpPort { get; set; }

    [Required]
    public string SmtpUser { get; set; } = string.Empty;

    [Required]
    public string SmtpPass { get; set; } = string.Empty;

    [Required]
    public string From { get; set; } = string.Empty;

    [Required]
    public string To { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;
}
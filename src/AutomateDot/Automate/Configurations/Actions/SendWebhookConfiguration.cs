using AutomateDot.Automate.Configurations;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Configurations;

public sealed class SendWebhookConfiguration : IActionConfiguration
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}
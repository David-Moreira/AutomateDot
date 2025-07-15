using AutomateDot.Components.Automation;

namespace AutomateDot.Configurations;

public sealed class AutomateDotWebhookTriggerConfiguration : ITriggerConfiguration
{
    public string TriggerEvent { get; set; } = string.Empty;
}
using AutomateDot.Components.Automation.Triggers;

namespace AutomateDot.Configurations;

public sealed class AutomateDotWebhookTriggerConfiguration : ITriggerConfiguration
{
    public string TriggerEvent { get; set; } = string.Empty;
}
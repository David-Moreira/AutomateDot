using AutomateDot.Components.Automate.Triggers;
using AutomateDot.Payloads;

namespace AutomateDot.Configurations;

public sealed class AutomateDotWebhookTriggerConfiguration : ITriggerConfiguration
{
    public string TriggerEvent { get; set; } = string.Empty;

    public Dictionary<string, string> GetPayloadFields()
    {
        var payloadFields = new Dictionary<string, string>();
        return PayloadFieldHelper.GetWithAdditionalPayloadMappingOptions(payloadFields);
    }
}
using Microsoft.AspNetCore.Components;

namespace AutomateDot.Components.Automation.Triggers;

public interface ITriggerConfiguration
{
    public Dictionary<string, string> GetPayloadFields()
    {
        return new();
    }
}

public abstract class TriggerFormBase<T> : ComponentBase
    where T : ITriggerConfiguration, new()
{
    [EditorRequired][Parameter] public T TriggerConfiguration { get; set; } = default!;
}
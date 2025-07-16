using AutomateDot.Components.Automation.Triggers;

using Microsoft.AspNetCore.Components;

namespace AutomateDot.Components.Automation.Actions;

public interface IActionConfiguration
{
}

public abstract class ActionFormBase<T> : ComponentBase
    where T : IActionConfiguration, new()
{
    /// <summary>
    /// The trigger is provided in order to enhance the action with additional information.
    /// i.e payload mapping
    /// </summary>
    [Parameter] public ITriggerConfiguration TriggerConfiguration { get; set; } = default!;

    [EditorRequired][Parameter] public T ActionConfiguration { get; set; } = default!;

    public Dictionary<string, string> GetPayloadFields()
    {
        return TriggerConfiguration?.GetPayloadFields() ?? new();
    }
}
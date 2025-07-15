using Microsoft.AspNetCore.Components;

namespace AutomateDot.Components.Automation;

public interface ITriggerConfiguration
{
}

public abstract class TriggerFormBase<T> : ComponentBase
    where T : ITriggerConfiguration, new()
{
    [EditorRequired][Parameter] public T TriggerConfiguration { get; set; } = default!;
}
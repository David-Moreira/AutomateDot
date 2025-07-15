using Microsoft.AspNetCore.Components;

namespace AutomateDot.Components.Automation;

public interface IActionConfiguration
{
}

public abstract class ActionFormBase<T> : ComponentBase
    where T : IActionConfiguration, new()
{
    [EditorRequired][Parameter] public T ActionConfiguration { get; set; } = default!;
}
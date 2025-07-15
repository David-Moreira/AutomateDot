namespace AutomateDot.Automate;

public static class AutomateFormRegistry
{
    public static readonly Dictionary<string, Type> TriggerForms = new();
    public static readonly List<AutomateActionFormDefinition> ActionForms = new();
}

public sealed class AutomateActionFormDefinition(string Name, Type ActionForm, Type ActionConfiguration)
{
    public string Name { get; } = Name;
    public Type ActionForm { get; } = ActionForm;
    public Type ActionConfiguration { get; } = ActionConfiguration;
}
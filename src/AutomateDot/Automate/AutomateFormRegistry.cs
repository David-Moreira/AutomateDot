namespace AutomateDot.Automate;

public static class AutomateFormRegistry
{
    public static readonly List<AutomateFormDefinition> TriggerForms = new();
    public static readonly List<AutomateFormDefinition> ActionForms = new();
}

public sealed class AutomateFormDefinition(string Id, string Name, Type Form, Type Configuration)
{
    public string Id { get; } = Id;
    public string Name { get; } = Name;
    public Type FormType { get; } = Form;
    public Type ConfigurationType { get; } = Configuration;
}
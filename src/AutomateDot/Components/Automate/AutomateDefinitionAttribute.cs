namespace AutomateDot.Components.Automate;

public class AutomateDefinitionAttribute : Attribute
{
    public string Id { get; }
    public string Name { get; }

    public AutomateDefinitionAttribute(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
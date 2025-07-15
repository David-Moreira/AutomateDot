namespace AutomateDot.Components.Automation;

public class AutomateActionNameAttribute : Attribute
{
    public string Name { get; }

    public AutomateActionNameAttribute(string name)
    {
        Name = name;
    }
}
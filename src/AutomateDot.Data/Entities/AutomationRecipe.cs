using AutomateDot.Data.Enums;

namespace AutomateDot.Data.Entities;

public sealed class AutomationRecipe : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public TriggerType TriggerType { get; set; }
    public ActionType ActionType { get; set; }
    public string TriggerConfiguration { get; set; } = string.Empty;
    public string ActionConfiguration { get; set; } = string.Empty;
}
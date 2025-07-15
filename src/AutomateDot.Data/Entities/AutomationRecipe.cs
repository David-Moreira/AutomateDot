using AutomateDot.Data.Enums;

namespace AutomateDot.Data.Entities;

public sealed class AutomationRecipe : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string TriggerConfiguration { get; set; } = string.Empty;
    public string ActionConfiguration { get; set; } = string.Empty;
}

public sealed class AutomationExecution : BaseEntity
{
    public int AutomationRecipeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExecutionStatus Status { get; set; }
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public AutomationRecipe AutomationRecipe { get; set; } = default!;
    public ICollection<AutomationExecutionEntry> AutomationExecutionEntries { get; set; } = default!;
}

public sealed class AutomationExecutionEntry : BaseEntity
{
    public int AutomationExecutionId { get; set; }
    public string Message { get; set; } = string.Empty;
}
using AutomateDot.Data.Enums;

namespace AutomateDot.Data.Entities;

public sealed class AutomateRecipe : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string TriggerConfiguration { get; set; } = string.Empty;
    public string ActionConfiguration { get; set; } = string.Empty;
}

public sealed class AutomateExecution : BaseEntity
{
    public int AutomateRecipeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExecutionStatus Status { get; set; }
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public AutomateRecipe AutomateRecipe { get; set; } = default!;
    public ICollection<AutomateExecutionEntry> AutomateExecutionEntries { get; set; } = default!;
}

public sealed class AutomateExecutionEntry : BaseEntity
{
    public int AutomateExecutionId { get; set; }
    public string Message { get; set; } = string.Empty;
    public AutomateExecution AutomateExecution { get; set; } = default!;
}
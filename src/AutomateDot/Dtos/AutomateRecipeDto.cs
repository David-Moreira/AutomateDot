using AutomateDot.Automate;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Dtos;

public class AutomateRecipeTriggerDto
{
    [Required]
    public string? Trigger { get; set; }

    public Type? FormType { get; set; }

    [ValidateComplexType]
    public object? Configuration { get; set; }

    public string GetConfiguration()
    {
        return System.Text.Json.JsonSerializer.Serialize(Configuration);
    }

    public void SetConfiguration(AutomateRecipe automationRecipe)
    {
        var definition = AutomateFormRegistry.TriggerForms.FirstOrDefault(x => x.Id == automationRecipe.Trigger);
        if (definition is not null)
        {
            Trigger = automationRecipe.Trigger;
            FormType = definition.FormType;
            Configuration = System.Text.Json.JsonSerializer.Deserialize(automationRecipe.TriggerConfiguration, definition.ConfigurationType);
        }
    }
}

public class AutomateRecipeActionDto
{
    [Required]
    public string? Action { get; set; }

    public Type? FormType { get; set; }

    [ValidateComplexType]
    public object? Configuration { get; set; }

    public string GetConfiguration()
    {
        return System.Text.Json.JsonSerializer.Serialize(Configuration);
    }

    public void SetConfiguration(AutomateRecipe automationRecipe)
    {
        var definition = AutomateFormRegistry.ActionForms.FirstOrDefault(x => x.Id == automationRecipe.Action);
        if (definition is not null)
        {
            Action = automationRecipe.Action;
            FormType = definition.FormType;
            Configuration = System.Text.Json.JsonSerializer.Deserialize(automationRecipe.ActionConfiguration, definition.ConfigurationType);
        }
    }
}

public class AutomateRecipeDefinitionDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class AutomateRecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public string TriggerName
        => AutomateFormRegistry.TriggerForms.FirstOrDefault(x => x.Id == Trigger)?.Name ?? string.Empty;

    public string ActionName
        => AutomateFormRegistry.ActionForms.FirstOrDefault(x => x.Id == Action)?.Name ?? string.Empty;
}

public sealed class AutomateExecutionDto
{
    public int Id { get; set; }
    public int AutomateRecipeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExecutionStatus Status { get; set; }
    public string Trigger { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public string TriggerName
        => AutomateFormRegistry.TriggerForms.FirstOrDefault(x => x.Id == Trigger)?.Name ?? string.Empty;

    public string ActionName
        => AutomateFormRegistry.ActionForms.FirstOrDefault(x => x.Id == Action)?.Name ?? string.Empty;

    public ICollection<AutomateExecutionEntryDto> AutomateExecutionEntries { get; set; } = default!;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}

public sealed class AutomateExecutionEntryDto
{
    public int Id { get; set; }
    public int AutomateExecutionId { get; set; }
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}
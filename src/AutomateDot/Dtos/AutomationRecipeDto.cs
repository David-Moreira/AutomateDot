using AutomateDot.Configurations;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

using System.ComponentModel.DataAnnotations;

namespace AutomateDot.Dtos;

public class AutomationRecipeTriggerDto
{
    private TriggerType _triggerType;

    public TriggerType TriggerType
    {
        get => _triggerType;
        set
        {
            GitHubWebhookTriggerConfiguration = null;
            AutomateDotWebhookTriggerConfiguration = null;
            switch (value)
            {
                case TriggerType.GitHubWebhook:
                    GitHubWebhookTriggerConfiguration = new();
                    break;

                case TriggerType.AutomateDotWebhook:
                    AutomateDotWebhookTriggerConfiguration = new();
                    break;
            }
            _triggerType = value;
        }
    }

    [ValidateComplexType]
    public GitHubWebhookTriggerConfiguration? GitHubWebhookTriggerConfiguration { get; set; } = new();

    [ValidateComplexType]
    public AutomateDotWebhookTriggerConfiguration? AutomateDotWebhookTriggerConfiguration { get; set; }

    public string GetTriggerConfiguration()
    {
        switch (TriggerType)
        {
            case TriggerType.GitHubWebhook:
                return System.Text.Json.JsonSerializer.Serialize(GitHubWebhookTriggerConfiguration);

            case TriggerType.AutomateDotWebhook:
                return System.Text.Json.JsonSerializer.Serialize(AutomateDotWebhookTriggerConfiguration);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetTriggerConfiguration(AutomationRecipe automationRecipe)
    {
        TriggerType = automationRecipe.TriggerType;
        switch (automationRecipe.TriggerType)
        {
            case TriggerType.GitHubWebhook:
                GitHubWebhookTriggerConfiguration = System.Text.Json.JsonSerializer.Deserialize<GitHubWebhookTriggerConfiguration>(automationRecipe.TriggerConfiguration);
                break;

            case TriggerType.AutomateDotWebhook:
                AutomateDotWebhookTriggerConfiguration = System.Text.Json.JsonSerializer.Deserialize<AutomateDotWebhookTriggerConfiguration>(automationRecipe.TriggerConfiguration);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public class AutomationRecipeActionDto
{
    public string? Action { get; set; }

    public Type? FormType { get; set; }

    [ValidateComplexType]
    public object? Configuration { get; set; }

    public string GetConfigurationName()
    {
        return Configuration?.GetType()?.AssemblyQualifiedName ?? string.Empty;
    }

    public string GetConfiguration()
    {
        return System.Text.Json.JsonSerializer.Serialize(Configuration);
    }

    public void SetConfiguration(AutomationRecipe automationRecipe)
    {
    }
}

public class AutomationRecipeDefinitionDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}

public class AutomationRecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public TriggerType TriggerType { get; set; }
    public ActionType ActionType { get; set; }
}
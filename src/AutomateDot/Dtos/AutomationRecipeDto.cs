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
    private ActionType _actionType;

    public ActionType ActionType
    {
        get => _actionType;
        set
        {
            SendWebhookConfiguration = null;
            GotifyConfiguration = null;
            ScriptConfiguration = null;
            switch (value)
            {
                case Data.Enums.ActionType.SendWebhook:
                    SendWebhookConfiguration = new();
                    break;

                case Data.Enums.ActionType.Gotify:
                    GotifyConfiguration = new();
                    break;

                case Data.Enums.ActionType.Script:
                    ScriptConfiguration = new();
                    break;
            }
            _actionType = value;
        }
    }

    [ValidateComplexType]
    public SendWebhookConfiguration? SendWebhookConfiguration { get; set; } = new();

    [ValidateComplexType]
    public GotifyConfiguration? GotifyConfiguration { get; set; }

    [ValidateComplexType]
    public ScriptConfiguration? ScriptConfiguration { get; set; }

    public string GetActionConfiguration()
    {
        switch (ActionType)
        {
            case Data.Enums.ActionType.SendWebhook:
                return System.Text.Json.JsonSerializer.Serialize(SendWebhookConfiguration);

            case Data.Enums.ActionType.Gotify:
                return System.Text.Json.JsonSerializer.Serialize(GotifyConfiguration);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetActionConfiguration(AutomationRecipe automationRecipe)
    {
        ActionType = automationRecipe.ActionType;
        switch (automationRecipe.ActionType)
        {
            case Data.Enums.ActionType.SendWebhook:
                SendWebhookConfiguration = System.Text.Json.JsonSerializer.Deserialize<SendWebhookConfiguration>(automationRecipe.ActionConfiguration);
                break;

            case Data.Enums.ActionType.Gotify:
                GotifyConfiguration = System.Text.Json.JsonSerializer.Deserialize<GotifyConfiguration>(automationRecipe.ActionConfiguration);
                break;

            case Data.Enums.ActionType.Script:
                ScriptConfiguration = System.Text.Json.JsonSerializer.Deserialize<ScriptConfiguration>(automationRecipe.ActionConfiguration);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
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
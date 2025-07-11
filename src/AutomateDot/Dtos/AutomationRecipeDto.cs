using AutomateDot.Configurations;
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
            switch (value)
            {
                case Data.Enums.ActionType.SendWebhook:
                    SendWebhookConfiguration = new();
                    break;

                case Data.Enums.ActionType.Gotify:
                    GotifyConfiguration = new();
                    break;
            }
            _actionType = value;
        }
    }

    [ValidateComplexType]
    public SendWebhookConfiguration? SendWebhookConfiguration { get; set; } = new();

    [ValidateComplexType]
    public GotifyConfiguration? GotifyConfiguration { get; set; }

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
}

public class AutomationRecipeDefinitionDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}

public class AutomationRecipeDto
{
    private TriggerType _triggerType;
    private ActionType _actionType;

    [Required]
    public string Name { get; set; } = string.Empty;

    public TriggerType TriggerType
    {
        get => _triggerType;
        set
        {
            GitHubWebhookTriggerConfiguration = null;
            switch (value)
            {
                case TriggerType.GitHubWebhook:
                    GitHubWebhookTriggerConfiguration = new();
                    break;
            }
            _triggerType = value;
        }
    }

    public ActionType ActionType
    {
        get => _actionType;
        set
        {
            SendWebhookConfiguration = null;
            GotifyConfiguration = null;
            switch (value)
            {
                case Data.Enums.ActionType.SendWebhook:
                    SendWebhookConfiguration = new();
                    break;

                case Data.Enums.ActionType.Gotify:
                    GotifyConfiguration = new();
                    break;
            }
            _actionType = value;
        }
    }

    [ValidateComplexType]
    public GitHubWebhookTriggerConfiguration? GitHubWebhookTriggerConfiguration { get; set; } = new();

    [ValidateComplexType]
    public SendWebhookConfiguration? SendWebhookConfiguration { get; set; } = new();

    [ValidateComplexType]
    public GotifyConfiguration? GotifyConfiguration { get; set; }

    public string GetTriggerConfiguration()
    {
        switch (TriggerType)
        {
            case TriggerType.GitHubWebhook:
                return System.Text.Json.JsonSerializer.Serialize(GitHubWebhookTriggerConfiguration);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

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
}
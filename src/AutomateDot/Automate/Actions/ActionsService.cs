using AutomateDot.Configurations;
using AutomateDot.Data;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

namespace AutomateDot.Actions;

public class AutomationExecutionService(ApplicationDbContext ApplicationDbContext)
{
    public async Task<int> StartExecution(AutomationRecipe recipe)
    {
        var execution = await ApplicationDbContext.AutomationExecutions.AddAsync(new AutomationExecution()
        {
            AutomationRecipeId = recipe.Id,
            Name = recipe.Name,
            TriggerType = recipe.TriggerType,
            ActionType = recipe.ActionType,
        });
        await ApplicationDbContext.SaveChangesAsync();
        return execution.Entity.Id;
    }

    public async Task Log(int executionId, string message)
    {
        await ApplicationDbContext.AutomationExecutionEntries.AddAsync(new AutomationExecutionEntry()
        {
            AutomationExecutionId = executionId,
            Message = message,
        });
        await ApplicationDbContext.SaveChangesAsync();
    }
}

public class ActionsService(AutomationExecutionService AutomationExecutionService, ILogger<ActionsService> Logger, SendWebhookAction SendWebhookAction, GotifyAction GotifyAction, ScriptAction ScriptAction)
{
    public async Task Execute(AutomationRecipe recipe)
    {
        var executionId = await AutomationExecutionService.StartExecution(recipe);

        try
        {
            var actionType = recipe.ActionType;
            var configuration = recipe.ActionConfiguration;

            switch (actionType)
            {
                case ActionType.SendWebhook:
                    {
                        var config = System.Text.Json.JsonSerializer.Deserialize<SendWebhookConfiguration>(configuration);
                        await SendWebhookAction.ExecuteAsync(config!);
                        break;
                    }
                case ActionType.Gotify:
                    {
                        var config = System.Text.Json.JsonSerializer.Deserialize<GotifyConfiguration>(configuration);
                        await GotifyAction.ExecuteAsync(config!);
                        break;
                    }
                case ActionType.Script:
                    {
                        var config = System.Text.Json.JsonSerializer.Deserialize<ScriptConfiguration>(configuration);
                        await ScriptAction.ExecuteAsync(config!);
                        break;
                    }
                default:
                    {
                        await AutomationExecutionService.Log(executionId, "Could not find anything to execute for this action type");
                        break;
                    }
            }
            await AutomationExecutionService.Log(executionId, "Executed successfully");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error has occurred");
            await AutomationExecutionService.Log(executionId, "An error has occurred");
        }
    }
}
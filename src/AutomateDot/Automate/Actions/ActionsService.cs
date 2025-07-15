using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;
using AutomateDot.Services;

namespace AutomateDot.Actions;

public class ActionsService(AutomationExecutionService AutomationExecutionService, ILogger<ActionsService> Logger, SendWebhookAction SendWebhookAction, GotifyAction GotifyAction, ScriptAction ScriptAction)
{
    public async Task Execute(AutomationRecipe recipe)
    {
        var executionId = await AutomationExecutionService.StartExecution(recipe);

        try
        {
            var actionType = recipe.Action;
            var configuration = recipe.ActionConfiguration;

            await AutomationExecutionService.Log(executionId, "Executed successfully");
            await AutomationExecutionService.UpdateExecution(executionId, ExecutionStatus.Success);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error has occurred");
            await AutomationExecutionService.Log(executionId, "An error has occurred");
            await AutomationExecutionService.UpdateExecution(executionId, ExecutionStatus.Failure);
        }
    }
}
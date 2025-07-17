using AutomateDot.Automate;
using AutomateDot.Automate.Configurations;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;
using AutomateDot.Services;

namespace AutomateDot.Actions;

public class ActionsService(AutomateExecutionService AutomateExecutionService, ILogger<ActionsService> Logger, IServiceProvider ServiceProvider)
{
    public async Task Execute(AutomateRecipe recipe, object? triggerPayload = null)
    {
        var executionId = await AutomateExecutionService.StartExecution(recipe);

        try
        {
            var configuration = recipe.ActionConfiguration;

            var definition = AutomateFormRegistry.ActionForms.FirstOrDefault(x => x.Id == recipe.Action);

            if (definition is null)
            {
                await LogFailure(executionId, "Action definition not found");
                return;
            }
            var actionConfiguration = System.Text.Json.JsonSerializer.Deserialize(recipe.ActionConfiguration, definition.ConfigurationType) as IActionConfiguration;
            if (actionConfiguration is null)
            {
                await LogFailure(executionId, "Action configuration is null");
                return;
            }
            var actionHandler =
                ServiceProvider.GetRequiredService(definition.Handler!);

            var method = actionHandler.GetType().GetMethod("ExecuteAsync", [definition.ConfigurationType, typeof(object)]);

            if (method is not null && actionConfiguration is not null)
            {
                var task = (Task)method.Invoke(actionHandler, [actionConfiguration, triggerPayload])!;
                await task;
            }
            else
            {
                await LogFailure(executionId, "Could not invoke ExecuteAsync on action handler");
                return;
            }

            await LogSuccess(executionId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error has occurred");
            await LogFailure(executionId, "An unexpected error has occurred, please check the application logs");
        }
    }

    private async Task LogSuccess(int executionId)
    {
        await AutomateExecutionService.Log(executionId, "Executed successfully");
        await AutomateExecutionService.UpdateExecution(executionId, ExecutionStatus.Success);
    }

    private async Task LogFailure(int executionId, string message)
    {
        await AutomateExecutionService.Log(executionId, message);
        await AutomateExecutionService.UpdateExecution(executionId, ExecutionStatus.Failure);
    }
}
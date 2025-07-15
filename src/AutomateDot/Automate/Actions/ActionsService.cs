using AutomateDot.Automate;
using AutomateDot.Components.Automation;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;
using AutomateDot.Services;

namespace AutomateDot.Actions;

public interface IActionHandler<in T> where T : IActionConfiguration
{
    Task ExecuteAsync(T config);
}

public class ActionsService(AutomationExecutionService AutomationExecutionService, ILogger<ActionsService> Logger, IServiceProvider ServiceProvider)
{
    public async Task Execute(AutomationRecipe recipe)
    {
        var executionId = await AutomationExecutionService.StartExecution(recipe);

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

            var method = actionHandler.GetType().GetMethod("ExecuteAsync", [definition.ConfigurationType]);

            if (method is not null && actionConfiguration is not null)
            {
                var task = (Task)method.Invoke(actionHandler, [actionConfiguration])!;
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
        await AutomationExecutionService.Log(executionId, "Executed successfully");
        await AutomationExecutionService.UpdateExecution(executionId, ExecutionStatus.Success);
    }

    private async Task LogFailure(int executionId, string message)
    {
        await AutomationExecutionService.Log(executionId, message);
        await AutomationExecutionService.UpdateExecution(executionId, ExecutionStatus.Failure);
    }
}
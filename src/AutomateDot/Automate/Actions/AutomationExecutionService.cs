using AutomateDot.Data;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

using Microsoft.EntityFrameworkCore;

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
            Status = ExecutionStatus.InProgress
        });
        await ApplicationDbContext.SaveChangesAsync();
        return execution.Entity.Id;
    }

    public async Task UpdateExecution(int executionId, ExecutionStatus status)
    {
        await ApplicationDbContext.AutomationExecutions
            .Where(x => x.Id == executionId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, status));
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

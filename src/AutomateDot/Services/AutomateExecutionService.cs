using AutomateDot.Data;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Services;

public class AutomateExecutionService(ApplicationDbContext ApplicationDbContext)
{
    public async Task<int> StartExecution(AutomateRecipe recipe)
    {
        var execution = await ApplicationDbContext.AutomateExecutions.AddAsync(new AutomateExecution()
        {
            AutomateRecipeId = recipe.Id,
            Name = recipe.Name,
            Trigger = recipe.Trigger,
            Action = recipe.Action,
            Status = ExecutionStatus.InProgress
        });
        await ApplicationDbContext.SaveChangesAsync();
        return execution.Entity.Id;
    }

    public async Task UpdateExecution(int executionId, ExecutionStatus status)
    {
        await ApplicationDbContext.AutomateExecutions
            .Where(x => x.Id == executionId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, status));
    }

    public async Task Log(int executionId, string message)
    {
        await ApplicationDbContext.AutomateExecutionEntries.AddAsync(new AutomateExecutionEntry()
        {
            AutomateExecutionId = executionId,
            Message = message,
        });
        await ApplicationDbContext.SaveChangesAsync();
    }
}
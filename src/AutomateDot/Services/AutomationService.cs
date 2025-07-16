using AutomateDot.Actions;
using AutomateDot.Automate;
using AutomateDot.Configurations;
using AutomateDot.Data;
using AutomateDot.Data.Entities;

using Hangfire;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Services;

public class AutomationService(ApplicationDbContext ApplicationDbContext, ActionsService ActionsService, ILogger<AutomationService> Logger)
{
    public async Task<AutomationRecipe> Get(int id)
    {
        return await ApplicationDbContext.AutomationRecipes.FirstAsync(x => x.Id == id);
    }

    public async Task<List<AutomationRecipe>> GetAll()
    {
        return await ApplicationDbContext.AutomationRecipes
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<AutomationRecipe>> GetByTriggerId(string id)
    {
        return await ApplicationDbContext.AutomationRecipes
            .Where(x => x.Trigger == id)
            .ToListAsync();
    }

    public async Task Add(AutomationRecipe recipe)
    {
        await ApplicationDbContext.AddAsync(recipe);
        await ApplicationDbContext.SaveChangesAsync();

        if (recipe.Trigger == AutomateDot.Automate.Constants.Triggers.CRON)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == AutomateDot.Automate.Constants.Triggers.CRON);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as CronTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomationService>(
                recipe.Id.ToString(),
                x => x.ExecuteRecipeAction(recipe.Id), configuration!.CronExpression);
            }
        }
    }

    public async Task Update(AutomationRecipe recipe)
    {
        ApplicationDbContext.AutomationRecipes.Update(recipe);
        await ApplicationDbContext.SaveChangesAsync();

        if (recipe.Trigger == AutomateDot.Automate.Constants.Triggers.CRON)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == AutomateDot.Automate.Constants.Triggers.CRON);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as CronTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomationService>(
                recipe.Id.ToString(),
                x => x.ExecuteRecipeAction(recipe.Id), configuration!.CronExpression);
            }
        }
        else
        {
            RecurringJob.RemoveIfExists(recipe.Id.ToString());
        }
    }

    public async Task ExecuteRecipeAction(int id)
    {
        var recipe = await ApplicationDbContext
            .AutomationRecipes.FirstOrDefaultAsync(x => x.Id == id);

        if (recipe is null)
        {
            Logger.LogError("Could not execute recipe action, not found for {recipe}", id);
            return;
        }

        await ActionsService.Execute(recipe);
    }
}

public interface IHangfireJobHandler
{
    public Task ExecuteRecipeAction(int id);
}
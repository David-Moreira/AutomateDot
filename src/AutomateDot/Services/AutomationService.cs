using AutomateDot.Actions;
using AutomateDot.Automate;
using AutomateDot.Configurations;
using AutomateDot.Data;
using AutomateDot.Data.Entities;
using AutomateDot.Triggers;

using Hangfire;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Services;

public class AutomateService(ApplicationDbContext ApplicationDbContext, ActionsService ActionsService, ILogger<AutomateService> Logger, PingTrigger PingTrigger)
{
    public async Task<AutomateRecipe?> Get(int id)
    {
        return await ApplicationDbContext.AutomateRecipes.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<AutomateRecipe>> GetAll()
    {
        return await ApplicationDbContext.AutomateRecipes
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<AutomateRecipe>> GetByTriggerId(string id)
    {
        return await ApplicationDbContext.AutomateRecipes
            .Where(x => x.Trigger == id)
            .ToListAsync();
    }

    public async Task Add(AutomateRecipe recipe)
    {
        await ApplicationDbContext.AddAsync(recipe);
        await ApplicationDbContext.SaveChangesAsync();

        if (recipe.Trigger == Constants.Triggers.CRON && recipe.IsActive)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == Constants.Triggers.CRON);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as CronTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomateService>(
                recipe.Id.ToString(),
                x => x.ExecuteAction(recipe.Id, null), configuration!.CronExpression);
            }
        }
        else if (recipe.Trigger == Constants.Triggers.PING && recipe.IsActive)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == Constants.Triggers.PING);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as PingTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomateService>(
                recipe.Id.ToString(),
                x => x.ExecutePingTrigger(recipe.Id, configuration!), $"*/{configuration!.Minutes} * * * *");
            }
        }
    }

    public async Task Update(AutomateRecipe recipe)
    {
        ApplicationDbContext.AutomateRecipes.Update(recipe);
        await ApplicationDbContext.SaveChangesAsync();

        if (recipe.Trigger == Constants.Triggers.CRON && recipe.IsActive)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == Constants.Triggers.CRON);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as CronTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomateService>(
                recipe.Id.ToString(),
                x => x.ExecuteAction(recipe.Id, null), configuration!.CronExpression);
            }
        }
        else if (recipe.Trigger == Constants.Triggers.PING && recipe.IsActive)
        {
            //Hard coded for now, but should be configurable per Form,
            //allowing access to Hangfire for scheduling
            var definition = AutomateFormRegistry.
                TriggerForms.FirstOrDefault(x => x.Id == Constants.Triggers.PING);
            if (definition is not null)
            {
                var configuration =
                    System.Text.Json.JsonSerializer.Deserialize(recipe.TriggerConfiguration, definition.ConfigurationType)
                    as PingTriggerConfiguration;

                RecurringJob.AddOrUpdate<AutomateService>(
                recipe.Id.ToString(),
                x => x.ExecutePingTrigger(recipe.Id, configuration!), $"*/{configuration!.Minutes} * * * *");
            }
        }
        else
        {
            RecurringJob.RemoveIfExists(recipe.Id.ToString());
        }
    }

    public async Task ExecutePingTrigger(int id, PingTriggerConfiguration pingTriggerConfiguration)
    {
        var result = await PingTrigger.ShouldTrigger(pingTriggerConfiguration);
        if (result)
        {
            await ExecuteAction(id, null);
        }
    }

    public async Task ExecuteAction(int id, object? triggerPayload = null)
    {
        var recipe = await ApplicationDbContext
            .AutomateRecipes.FirstOrDefaultAsync(x => x.Id == id);

        if (recipe is null)
        {
            Logger.LogError("Could not execute recipe action, not found for {recipe}", id);
            return;
        }

        await ExecuteAction(recipe, triggerPayload);
    }

    public async Task ExecuteAction(AutomateRecipe recipe, object? triggerPayload = null)
    {
        await ActionsService.Execute(recipe, triggerPayload);
    }
}

public interface IHangfireJobHandler
{
    public Task ExecuteRecipeAction(int id);
}
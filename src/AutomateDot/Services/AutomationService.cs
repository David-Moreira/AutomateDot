using AutomateDot.Data;
using AutomateDot.Data.Entities;
using AutomateDot.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Services;

public class AutomationService(ApplicationDbContext ApplicationDbContext)
{
    public async Task<List<AutomationRecipe>> GetByTriggerType(TriggerType triggerType)
    {
        return await ApplicationDbContext.AutomationRecipes
            .Where(x => x.TriggerType == triggerType)
            .ToListAsync();
    }
}
using AutomateDot.Data;
using AutomateDot.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Services;

public class AutomationService(ApplicationDbContext ApplicationDbContext)
{
    public async Task<List<AutomationRecipe>> GetByTriggerId(string id)
    {
        return await ApplicationDbContext.AutomationRecipes
            .Where(x => x.Trigger == id)
            .ToListAsync();
    }
}
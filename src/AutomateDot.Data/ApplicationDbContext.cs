using AutomateDot.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace AutomateDot.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AutomateRecipe> AutomateRecipes { get; set; }
    public DbSet<AutomateExecution> AutomateExecutions { get; set; }
    public DbSet<AutomateExecutionEntry> AutomateExecutionEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not BaseEntity baseEntity)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                baseEntity.CreatedOn = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                baseEntity.UpdatedOn = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
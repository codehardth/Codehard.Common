using LanguageExt;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static Aff<int> SaveChangesAff(
        this DbContext dbContext, CancellationToken ct = default)
    {
        return
            Aff(async () => await dbContext.SaveChangesAsync(ct));
    }
    
    public static Eff<int> SaveChangesEff(this DbContext dbContext)
    {
        return
            Eff(() => dbContext.SaveChanges());
    }

    public static Eff<EntityEntry<TEntity>> AddEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Add(entity));
    }
    
    public static Eff<EntityEntry<TEntity>> UpdateEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Update(entity));
    }
    
    public static Eff<EntityEntry<TEntity>> RemoveEff<TEntity>(
        this DbContext dbContext, TEntity entity)
        where TEntity : class
    {
        return
            Eff(() => dbContext.Remove(entity));
    }
}
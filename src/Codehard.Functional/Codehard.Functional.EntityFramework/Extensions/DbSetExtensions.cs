using LanguageExt;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static Eff<EntityEntry<TEntity>> AddEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return Eff(() => dbSet.Add(entity));
    }
    
    public static Eff<Unit> AddRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbSet.AddRange(entities);

            return unit;
        });
    }
    
    public static Eff<EntityEntry<TEntity>> UpdateEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return Eff(() => dbSet.Update(entity));
    }
    
    public static Eff<Unit> UpdateRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbSet.UpdateRange(entities);

            return unit;
        });
    }
    
    public static Eff<EntityEntry<TEntity>> RemoveEff<TEntity>(
        this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class
    {
        return Eff(() => dbSet.Remove(entity));
    }
    
    public static Eff<Unit> RemoveRangeEff<TEntity>(
        this DbSet<TEntity> dbSet, params TEntity[] entities)
        where TEntity : class
    {
        return Eff(() =>
        {
            dbSet.RemoveRange(entities);

            return unit;
        });
    }
}
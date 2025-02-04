using System.Linq.Expressions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

using Codehard.Common.DomainModel;

using static LanguageExt.Prelude;

namespace Codehard.Functional.EntityFramework.Extensions;

public static class QueryablePrelude
{
    /// <summary>
    /// Asynchronously converts a sequence to a list within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps a <see cref="System.Collections.Generic.List{T}"/> that contains elements from the input sequence.
    /// </returns>
    public static Eff<IQueryable<T>, List<T>> ToListEff<T>()
    {
        return
            LanguageExt.Eff<IQueryable<T>, List<T>>.LiftIO(
                static source =>
                    liftIO(env => source.ToListAsync(env.Token)));
    }
    
    /// <summary>
    /// Asynchronously converts a sequence to an array within an Eff monad.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <returns>
    /// An Eff monad that represents the asynchronous operation. The Eff monad wraps an array that contains elements from the input sequence.
    /// </returns>
    public static Eff<IQueryable<T>, T[]> ToArrayEff<T>()
    {
        return
            LanguageExt.Eff<IQueryable<T>, T[]>.LiftIO(
                static source =>
                    liftIO(env => source.ToArrayAsync(env.Token)));
    }
    
    public static Eff<DbContext, IQueryable<T>> From<T>()
        where T : class
    {
        return
            LanguageExt.Eff<DbContext, IQueryable<T>>.Lift(
                dbContext => dbContext.Set<T>().AsQueryable());
    }
    
    public static Eff<DbSet<TEntity>, Option<TEntity>> FindByKey<TEntity, TKey>(
        TKey key)
        where TEntity : class, IEntity<TKey>
        where TKey : struct
    {
        return
            LanguageExt.Eff<DbSet<TEntity>, Option<TEntity>>.LiftIO(
                source =>
                    liftIO(async env => Optional(await source.FindAsync(
                        new object[] { key },
                        cancellationToken: env.Token))));
    }
}
using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Marten;

/// <summary>
/// 
/// </summary>
public static class DocumentSessionExtensions
{
    /// <summary>
    /// Save changes to the database
    /// </summary>
    public static Eff<Unit> SaveChangesEff(
        this IDocumentSession documentSession, CancellationToken cancellationToken = default)
    {
        return liftEff(
            async () =>
            await documentSession.SaveChangesAsync(cancellationToken).ToUnit());
    }
    
    /// <summary>
    /// Eff monad wrapper for storing entities in Marten's IDocumentSession.
    /// </summary>
    /// <typeparam name="T">The type of the entities to store.</typeparam>
    /// <param name="documentSession">The Marten IDocumentSession to use for storing entities.</param>
    /// <param name="entities">The entities to store in the document session.</param>
    /// <returns>An Eff&lt;Unit&gt; representing the side effect of storing entities in the document session.</returns>
    public static Eff<Unit> StoreEff<T>(this IDocumentSession documentSession, params T[] entities)
        where T : notnull
    {
        return liftEff(() =>
        {
            documentSession.Store(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Eff monad wrapper for updating entities in Marten's IDocumentSession.
    /// </summary>
    /// <typeparam name="T">The type of the entities to update.</typeparam>
    /// <param name="documentSession">The Marten IDocumentSession to use for updating entities.</param>
    /// <param name="entities">The entities to update in the document session.</param>
    /// <returns>An Eff&lt;Unit&gt; representing the side effect of updating entities in the document session.</returns>
    public static Eff<Unit> UpdateEff<T>(this IDocumentSession documentSession, params T[] entities)
        where T : notnull
    {
        return liftEff(() =>
        {
            documentSession.Update(entities);

            return unit;
        });
    }
    
    /// <summary>
    /// Eff monad wrapper for updating entities in Marten's IDocumentSession.
    /// </summary>
    /// <typeparam name="T">The type of the entities to update.</typeparam>
    /// <param name="documentSession">The Marten IDocumentSession to use for updating entities.</param>
    /// <param name="entity">The entity to update in the document session.</param>
    /// <returns>An Eff&lt;Unit&gt; representing the side effect of updating entities in the document session.</returns>
    public static Eff<Unit> DeleteEff<T>(this IDocumentSession documentSession, T entity)
        where T : notnull
    {
        return liftEff(() =>
        {
            documentSession.Delete(entity);

            return unit;
        });
    }
    
    /// <summary>
    /// Eff monad wrapper for deleting an entity by its identifier from Marten's IDocumentSession.
    /// </summary>
    /// <typeparam name="T">The type of the entity to delete.</typeparam>
    /// <param name="documentSession">The Marten IDocumentSession to use for deleting the entity.</param>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>An Eff&lt;Unit&gt; representing the side effect of deleting the entity from the document session.</returns>
    public static Eff<Unit> DeleteEff<T>(this IDocumentSession documentSession, Guid id)
        where T : notnull
    {
        return liftEff(() =>
        {
            documentSession.Delete<T>(id);

            return unit;
        });
    }
}
using Codehard.Common.DomainModel;
using Codehard.Infrastructure.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// Extension methods for <see cref="DbContextOptionsBuilder"/> to register a domain event publisher interceptor.
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Registers a domain event publisher interceptor for the <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to which the interceptor is added.</param>
    /// <param name="delegate">The delegate responsible for asynchronously publishing domain events (optional).</param>
    public static void AddDomainEventPublisherInterceptor(
        this DbContextOptionsBuilder optionsBuilder,
        PublishDomainEventDelegate? @delegate = default)
    {
        optionsBuilder.AddInterceptors(new DomainEventPublisherInterceptor(@delegate));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="delegate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TMaterializedView"></typeparam>
    public static void AddEntityToMaterializedViewInterceptor<TEntity, TMaterializedView>(
        this DbContextOptionsBuilder optionsBuilder,
        MaterializedEntityTransformerDelegate<TEntity, TMaterializedView> @delegate)
        where TEntity : class, IAggregateRoot
        where TMaterializedView : class
    {
        optionsBuilder.AddInterceptors(new EntityToMaterializedViewInterceptor<TEntity, TMaterializedView>(@delegate));
    }
}
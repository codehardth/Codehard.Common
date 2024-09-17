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
    public static DbContextOptionsBuilder AddDomainEventPublisherInterceptor(
        this DbContextOptionsBuilder optionsBuilder,
        PublishDomainEventDelegate? @delegate = default)
    {
        return optionsBuilder.AddInterceptors(new DomainEventPublisherInterceptor(@delegate));
    }

    public static DbContextOptionsBuilder AddRecomputeMaterializedViewIntercetor<TEntity, TEvent>(
        this DbContextOptionsBuilder optionsBuilder,
        string refreshMaterializedViewQuery) where TEvent : IDomainEvent
        where TEntity : class, IEntity
    {
        return optionsBuilder.AddInterceptors(
            new RecomputeMaterializedViewInterceptor<TEntity, TEvent>(refreshMaterializedViewQuery));
    }

    public static DbContextOptionsBuilder<TContext> AddEntityToMaterializedViewInterceptor<TContext, TEntity,
        TMaterializedView>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        MaterializedEntityTransformerDelegate<TEntity, TMaterializedView> @delegate)
        where TContext : DbContext
        where TEntity : class, IAggregateRoot
        where TMaterializedView : class
    {
        return optionsBuilder.AddInterceptors(
            new EntityToMaterializedViewInterceptor<TEntity, TMaterializedView>(@delegate));
    }

    public static DbContextOptionsBuilder<TContext> AddEntityToMaterializedViewInterceptor<TContext, TEntity, TRelate1,
        TMaterializedView>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        MaterializedEntityTransformerDelegate<TEntity, TRelate1, TMaterializedView> @delegate)
        where TContext : DbContext
        where TEntity : class, IAggregateRoot
        where TRelate1 : class, IEntity
        where TMaterializedView : class
    {
        return optionsBuilder.AddInterceptors(
            new EntityToMaterializedViewInterceptor<TEntity, TRelate1, TMaterializedView>(@delegate));
    }

    public static DbContextOptionsBuilder<TContext> AddEntityToMaterializedViewInterceptor<TContext, TEntity, TRelate1,
        TRelate2, TMaterializedView>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        MaterializedEntityTransformerDelegate<TEntity, TRelate1, TRelate2, TMaterializedView> @delegate)
        where TContext : DbContext
        where TEntity : class, IAggregateRoot
        where TRelate1 : class, IEntity
        where TRelate2 : class, IEntity
        where TMaterializedView : class
    {
        return optionsBuilder.AddInterceptors(
            new EntityToMaterializedViewInterceptor<TEntity, TRelate1, TRelate2, TMaterializedView>(@delegate));
    }

    public static DbContextOptionsBuilder<TContext> AddEntityToMaterializedViewInterceptor<TContext, TEntity, TRelate1,
        TRelate2, TRelate3, TMaterializedView>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        MaterializedEntityTransformerDelegate<TEntity, TRelate1, TRelate2, TRelate3, TMaterializedView> @delegate)
        where TContext : DbContext
        where TEntity : class, IAggregateRoot
        where TRelate1 : class, IEntity
        where TRelate2 : class, IEntity
        where TRelate3 : class, IEntity
        where TMaterializedView : class
    {
        return optionsBuilder.AddInterceptors(
            new EntityToMaterializedViewInterceptor<TEntity, TRelate1, TRelate2, TRelate3, TMaterializedView>(
                @delegate));
    }
}
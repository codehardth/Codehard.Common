using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Codehard.Infrastructure.EntityFramework.Interceptors;

// 1 entity
public delegate TResult MaterializedEntityTransformerDelegate<in TBaseEntity, out TResult>(TBaseEntity entity)
    where TBaseEntity : class, IAggregateRoot;

// 2 entities
public delegate TResult MaterializedEntityTransformerDelegate<in TBaseEntity, in TRelateEntity, out TResult>(
    TBaseEntity entity1, TRelateEntity entity2)
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity : class, IEntity;

// 3 entities
public delegate TResult MaterializedEntityTransformerDelegate<in TBaseEntity, in TRelateEntity1, in TRelateEntity2,
    out TResult>(
    TBaseEntity entity1, TRelateEntity1 entity2, TRelateEntity2 entity3)
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity1 : class, IEntity
    where TRelateEntity2 : class, IEntity;

// 4 entities
public delegate TResult MaterializedEntityTransformerDelegate<in TBaseEntity, in TRelateEntity1, in TRelateEntity2,
    in TRelateEntity3, out TResult>(
    TBaseEntity entity1, TRelateEntity1 entity2, TRelateEntity2 entity3, TRelateEntity3 entity4)
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity1 : class, IEntity
    where TRelateEntity2 : class, IEntity
    where TRelateEntity3 : class, IEntity;

internal abstract class EntityToMaterializedViewInterceptor : SaveChangesInterceptor
{
    private readonly Type baseEntityType;
    private readonly Type[] dependentTypes;

    protected EntityToMaterializedViewInterceptor(Type baseEntityType, Type[] dependentTypes)
    {
        this.baseEntityType = baseEntityType;
        this.dependentTypes = dependentTypes;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;

        if (context is null)
        {
            return result;
        }

        var baseEntries = context.ChangeTracker.GetAddedOrModifiedEntries(this.baseEntityType).ToArray();

        if (!baseEntries.Any())
        {
            return result;
        }

        var dependents =
            this.dependentTypes.Length == 0
                ? Array.Empty<EntityEntry>()
                : this.dependentTypes
                      .SelectMany(t => context.ChangeTracker.Entries().Where(e => e.Entity.GetType() == t))
                      .GroupBy(t => t.Entity.GetType())
                      .Select(g => g.Last()) // Probably needs a comparer to sort properly?
                      .ToArray();

        var results = baseEntries.Select(e => this.Transform(e, dependents)).ToArray();

        context.AddRange(results);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(this.SavingChanges(eventData, result));
    }

    protected abstract object Transform(EntityEntry baseEntity, EntityEntry[] dependents);
}

// 1 entity
internal class EntityToMaterializedViewInterceptor<TBaseEntity, TResult> : EntityToMaterializedViewInterceptor
    where TBaseEntity : class, IAggregateRoot
    where TResult : class
{
    private readonly MaterializedEntityTransformerDelegate<TBaseEntity, TResult> mapper;

    internal EntityToMaterializedViewInterceptor(
        MaterializedEntityTransformerDelegate<TBaseEntity, TResult> mapper)
        : base(typeof(TBaseEntity), Array.Empty<Type>())
    {
        this.mapper = mapper;
    }

    protected override object Transform(EntityEntry baseEntity, EntityEntry[] dependents)
    {
        var entity = (TBaseEntity)baseEntity.Entity;
        return this.mapper(entity);
    }
}

// 2 entities
internal class EntityToMaterializedViewInterceptor<TBaseEntity, TRelateEntity, TResult> : EntityToMaterializedViewInterceptor
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity : class, IEntity
    where TResult : class
{
    private readonly MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity, TResult> mapper;

    internal EntityToMaterializedViewInterceptor(
        MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity, TResult> mapper)
        : base(typeof(TBaseEntity), new[] { typeof(TRelateEntity) })
    {
        this.mapper = mapper;
    }

    protected override object Transform(EntityEntry baseEntity, EntityEntry[] dependents)
    {
        var entity = (TBaseEntity)baseEntity.Entity;
        var ref1 = (TRelateEntity)dependents[0].Entity;
        return this.mapper(entity, ref1);
    }
}

// 3 entities
internal class EntityToMaterializedViewInterceptor<TBaseEntity, TRelateEntity1, TRelateEntity2, TResult> : EntityToMaterializedViewInterceptor
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity1 : class, IEntity
    where TRelateEntity2 : class, IEntity
    where TResult : class
{
    private readonly MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity1, TRelateEntity2, TResult> mapper;

    internal EntityToMaterializedViewInterceptor(
        MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity1, TRelateEntity2, TResult> mapper)
        : base(typeof(TBaseEntity), new[] { typeof(TRelateEntity1), typeof(TRelateEntity2) })
    {
        this.mapper = mapper;
    }

    protected override object Transform(EntityEntry baseEntity, EntityEntry[] dependents)
    {
        var entity = (TBaseEntity)baseEntity.Entity;
        var ref1 = (TRelateEntity1)dependents[0].Entity;
        var ref2 = (TRelateEntity2)dependents[1].Entity;
        return this.mapper(entity, ref1, ref2);
    }
}

// 4 entities
internal class EntityToMaterializedViewInterceptor<TBaseEntity, TRelateEntity1, TRelateEntity2, TRelateEntity3, TResult> : EntityToMaterializedViewInterceptor
    where TBaseEntity : class, IAggregateRoot
    where TRelateEntity1 : class, IEntity
    where TRelateEntity2 : class, IEntity
    where TRelateEntity3 : class, IEntity
    where TResult : class
{
    private readonly MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity1, TRelateEntity2, TRelateEntity3, TResult> mapper;

    internal EntityToMaterializedViewInterceptor(
        MaterializedEntityTransformerDelegate<TBaseEntity, TRelateEntity1, TRelateEntity2, TRelateEntity3, TResult> mapper)
        : base(typeof(TBaseEntity), new[] { typeof(TRelateEntity1), typeof(TRelateEntity2), typeof(TRelateEntity3) })
    {
        this.mapper = mapper;
    }

    protected override object Transform(EntityEntry baseEntity, EntityEntry[] dependents)
    {
        var entity = (TBaseEntity)baseEntity.Entity;
        var ref1 = (TRelateEntity1)dependents[0].Entity;
        var ref2 = (TRelateEntity2)dependents[1].Entity;
        var ref3 = (TRelateEntity3)dependents[2].Entity;
        return this.mapper(entity, ref1, ref2, ref3);
    }
}

internal static class MatViewInterceptorHelper
{
    public static IEnumerable<EntityEntry<T>> GetAddedOrModifiedEntries<T>(this ChangeTracker changeTracker)
        where T : class
    {
        return changeTracker.Entries<T>().Where(e => e.State is EntityState.Added or EntityState.Modified);
    }

    public static IEnumerable<EntityEntry> GetAddedOrModifiedEntries(this ChangeTracker changeTracker, Type entityType)
    {
        return changeTracker.Entries().Where(e =>
            e.Entity.GetType().IsEquivalentTo(entityType) &&
            e.State is EntityState.Added or EntityState.Modified);
    }

    public static IEnumerable<EntityEntry<T>> GetNonDeletedEntries<T>(this ChangeTracker changeTracker) where T : class
    {
        return changeTracker.Entries<T>().Where(e => e.State is not (EntityState.Deleted or EntityState.Detached));
    }
}
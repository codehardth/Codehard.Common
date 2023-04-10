using Codehard.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// A base class for EntityTypeConfiguration with default option
/// that mapping all enum properties to string. 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class EntityTypeConfigurationBase<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.MapEnumPropertiesToString();
        builder.MapEntityKeyPropertiesToType();
        builder.MapMoneyPropertiesToOwnedEntity();

        this.EntityConfigure(builder);
    }

    /// <summary>
    /// Configures the entity of type TEntity.
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void EntityConfigure(EntityTypeBuilder<TEntity> builder);
}

/// <summary>
/// A base class for EntityTypeConfiguration with default option
/// that mapping all enum properties to string. 
/// </summary>
/// <typeparam name="TEntity">The entity type that is being configured.</typeparam>
/// <typeparam name="TContext">The target DbContext.</typeparam>
public abstract class EntityTypeConfigurationBase<TEntity, TContext> : EntityTypeConfigurationBase<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    /// <summary>
    /// Gets the type of the DbContext that this configuration applies to.
    /// </summary>
    public Type ContextType => typeof(TContext);
}
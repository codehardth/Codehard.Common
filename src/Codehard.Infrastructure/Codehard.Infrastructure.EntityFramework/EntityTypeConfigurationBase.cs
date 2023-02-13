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
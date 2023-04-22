using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations.ImmutableEntities;

public class EntityAConfiguration : EntityTypeConfigurationBase<ImmutableEntityA, TestDbContext>
{
    private readonly ModelBuilder builder;

    public EntityAConfiguration(ModelBuilder builder)
    {
        this.builder = builder;
    }

    protected override void EntityConfigure(EntityTypeBuilder<ImmutableEntityA> builder)
    {
        this.builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value)
               .IsRequired();

        builder.HasMany(e => e.Bs)
               .WithOne(e => e.A)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
using Codehard.Functional.EntityFramework.Tests.Entities;
using Codehard.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Functional.EntityFramework.Tests.Configurations;

public class EntityAConfiguration : EntityTypeConfigurationBase<EntityA, TestDbContext>
{
    private readonly ModelBuilder builder;

    public EntityAConfiguration(ModelBuilder builder)
    {
        this.builder = builder;
    }

    protected override void EntityConfigure(EntityTypeBuilder<EntityA> builder)
    {
        this.builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value);
    }
}
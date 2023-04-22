using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations.ImmutableEntities;

public class EntityBConfiguration : EntityTypeConfigurationBase<ImmutableEntityB, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<ImmutableEntityB> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value)
               .IsRequired();

        builder.HasMany(e => e.Cs)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
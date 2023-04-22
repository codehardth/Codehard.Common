using Codehard.Infrastructure.EntityFramework.Tests.ImmutableEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations.ImmutableEntities;

public class EntityCConfiguration : EntityTypeConfigurationBase<ImmutableEntityC, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<ImmutableEntityC> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value)
               .IsRequired();
    }
}
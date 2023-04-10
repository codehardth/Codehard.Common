using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations;

public class EntityCConfiguration : EntityTypeConfigurationBase<EntityC, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<EntityC> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Value)
               .IsRequired();
    }
}
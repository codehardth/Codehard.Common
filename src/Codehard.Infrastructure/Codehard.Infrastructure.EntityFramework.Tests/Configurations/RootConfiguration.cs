using Codehard.Infrastructure.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Tests.Configurations;

public class RootConfiguration : EntityTypeConfigurationBase<Root, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<Root> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Value)
            .IsRequired();
        builder.OwnsMany(r => r.Children, childBuilder =>
        {
            childBuilder.HasKey(c => c.Id);
            childBuilder.Property(c => c.Value)
                .IsRequired();
        });
    }
}

public class MatViewRootConfiguration : EntityTypeConfigurationBase<MaterializedRoot, TestDbContext>
{
    protected override void EntityConfigure(EntityTypeBuilder<MaterializedRoot> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();
        builder.Property(r => r.RootId)
            .IsRequired();
        builder.Property(r => r.Value)
            .IsRequired();
        builder.Property(r => r.LastestChildValue)
            .IsRequired();
    }
}
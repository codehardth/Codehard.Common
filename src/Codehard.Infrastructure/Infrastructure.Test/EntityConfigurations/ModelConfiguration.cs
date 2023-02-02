using Codehard.Infrastructure.EntityFramework;
using Infrastructure.Test.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Test.EntityConfigurations;

public class ModelConfiguration : EntityTypeConfigurationBase<MyModel>
{
    protected override void EntityConfigure(EntityTypeBuilder<MyModel> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Value)
            .IsRequired();

        builder.HasMany(m => m.Childs)
            .WithOne();
    }
}

public class ChildModelConfiguration : EntityTypeConfigurationBase<ChildModel>
{
    protected override void EntityConfigure(EntityTypeBuilder<ChildModel> builder)
    {
        builder.HasKey(c => c.Id);
    }
}
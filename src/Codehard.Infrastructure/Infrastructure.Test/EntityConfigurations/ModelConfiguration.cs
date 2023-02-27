using System.Linq.Expressions;
using Codehard.Functional.EntityFramework;
using Codehard.Infrastructure.EntityFramework;
using Infrastructure.Test.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Test.EntityConfigurations;

public class ModelConfiguration : EntityTypeConfigurationBase<MyModel>
{
    protected override void EntityConfigure(EntityTypeBuilder<MyModel> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Value)
            .IsRequired();
        builder.HasOptionProperty(m => m.Number);
        builder.HasOptionProperty(m => m.Text);

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
using System.Linq.Expressions;
using Codehard.Infrastructure.EntityFramework;
using Infrastructure.Test.Entities;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Test.EntityConfigurations;

public class OptionConverter<T> : ValueConverter<Option<T>, T>
{
    public OptionConverter()
        : base(
            opt => opt.MatchUnsafe(s => s, () => default!),
            v => Optional(v))
    {
    }

    public OptionConverter(
        Expression<Func<Option<T>, T>> convertToProviderExpression,
        Expression<Func<T, Option<T>>> convertFromProviderExpression,
        ConverterMappingHints? mappingHints = null)
        : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public OptionConverter(
        Expression<Func<Option<T>, T>> convertToProviderExpression,
        Expression<Func<T, Option<T>>> convertFromProviderExpression,
        bool convertsNulls,
        ConverterMappingHints? mappingHints = null)
        : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}

public class ModelConfiguration : EntityTypeConfigurationBase<MyModel>
{
    protected override void EntityConfigure(EntityTypeBuilder<MyModel> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Value)
            .IsRequired();
        // builder.Property(m => m.NullableValue)
        //     .HasConversion(new OptionConverter<int>());
        builder.Property("number")
            .IsRequired(false);
        builder.Property("text")
            .IsRequired(false);

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
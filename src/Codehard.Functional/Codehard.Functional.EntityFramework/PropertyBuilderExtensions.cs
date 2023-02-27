using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static LanguageExt.Prelude;

namespace Codehard.Functional.EntityFramework;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder HasOptionProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression)
        where TEntity : class
    {
        var property =
            ((MemberExpression)propertyExpression.Body).Member;

        var propertyName = property.Name;
        var propertyType = propertyExpression.Body.Type;

        if (propertyType == null)
        {
            throw new Exception($"Unable to read type from {propertyName}");
        }

        if (!(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Option<>)))
        {
            throw new Exception("Property is not an option.");
        }

        var actualType = propertyType.GenericTypeArguments[0];
        var nullableType =
            actualType.IsPrimitive
                ? typeof(Nullable<>).MakeGenericType(actualType)
                : actualType;

        var backingField = $"_{propertyName.ToLowerInvariant()}";

        builder.Ignore(propertyName);

        return
            builder.Property(nullableType, backingField)
                .HasColumnName(propertyName)
                .IsRequired(false);
    }
}
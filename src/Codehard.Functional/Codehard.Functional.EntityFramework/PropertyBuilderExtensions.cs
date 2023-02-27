using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        var backingField = $"_{propertyName.ToLowerInvariant()}";
        var backingFieldInfo =
            property.DeclaringType?.GetProperty(backingField, BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingFieldInfo == null)
        {
            throw new Exception($"Unable to find backing field '{backingField}' in {property.DeclaringType}.");
        }

        builder.Ignore(propertyName);

        return
            builder.Property(backingFieldInfo.PropertyType, backingField)
                .HasColumnName(propertyName)
                .IsRequired(false);
    }
}
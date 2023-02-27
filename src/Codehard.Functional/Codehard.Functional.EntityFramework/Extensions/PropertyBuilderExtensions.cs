using System.Linq.Expressions;
using System.Reflection;
using Codehard.Functional.EntityFramework;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

public static class OptionPropertyBuilderExtensions
{
    /// <summary>
    /// Configure a property of <see cref="Option{TEntity}"/> using a backing field.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="backingField"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static PropertyBuilder HasOptionProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        string? backingField = default)
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

        backingField ??= property.GetBackingFieldName();

        var backingFieldInfo =
            property.DeclaringType?.GetField(backingField, BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingFieldInfo == null)
        {
            throw new Exception($"Unable to find backing field '{backingField}' in {property.DeclaringType}.");
        }

        var entityType = property.DeclaringType!;
        var cacheKey = (entityType, propertyName);

        if (ConfigurationCache.BackingField.ContainsKey(cacheKey))
        {
            ConfigurationCache.BackingField[cacheKey] = backingField;
        }
        else
        {
            ConfigurationCache.BackingField.Add(cacheKey, backingField);
        }

        builder.Ignore(propertyName);

        return
            builder.Property(backingFieldInfo.FieldType, backingField)
                .HasColumnName(propertyName)
                .IsRequired(false);
    }
}
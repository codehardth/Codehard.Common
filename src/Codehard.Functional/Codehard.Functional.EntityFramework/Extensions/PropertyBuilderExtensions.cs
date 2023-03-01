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
        var (backingFieldInfo, backingFieldName, propertyName) = GetBackingField(propertyExpression, backingField);

        builder.Ignore(propertyName);

        return
            builder.Property(backingFieldInfo.FieldType, backingFieldName)
                .HasColumnName(propertyName)
                .IsRequired(false);
    }

    /// <summary>
    /// Configure a property of <see cref="Option{TEntity}"/> using a backing field.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="backingField"></param>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TDependent"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static PropertyBuilder HasOptionProperty<TOwner, TDependent, TProperty>(
        this OwnedNavigationBuilder<TOwner, TDependent> builder,
        Expression<Func<TDependent, TProperty>> propertyExpression,
        string? backingField = default)
        where TOwner : class
        where TDependent : class
    {
        var (backingFieldInfo, backingFieldName, propertyName) = GetBackingField(propertyExpression, backingField);

        builder.Ignore(propertyName);

        var ownerName = builder.Metadata.PrincipalToDependent?.Name;

        return
            builder.Property(backingFieldInfo.FieldType, backingFieldName)
                .HasColumnName($"{ownerName}_{propertyName}")
                .IsRequired(false);
    }

    private static (FieldInfo BackingField, string BackingFieldName, string PropertyName) GetBackingField
        <TEntity, TProperty>(
            Expression<Func<TEntity, TProperty>> propertyExpression,
            string? backingField)
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

        var actualBackingField = backingField ?? property.GetBackingFieldName();

        var backingFieldInfo =
            property.DeclaringType?.GetField(actualBackingField, BindingFlags.Instance | BindingFlags.NonPublic);
        var genericType = propertyType.GenericTypeArguments[0];
        var expectedType = genericType.IsValueType ? typeof(Nullable<>).MakeGenericType(genericType) : genericType;

        if (backingFieldInfo == null || backingFieldInfo.FieldType != expectedType)
        {
            throw new Exception(
                $"Unable to find backing field '{actualBackingField}' with type {expectedType} in {property.DeclaringType}.");
        }

        var entityType = property.DeclaringType!;
        var cacheKey = (entityType.FullName, propertyName);

        if (ConfigurationCache.BackingField.ContainsKey(cacheKey))
        {
            ConfigurationCache.BackingField[cacheKey] = actualBackingField;
        }
        else
        {
            ConfigurationCache.BackingField.Add(cacheKey, actualBackingField);
        }

        return (backingFieldInfo, actualBackingField, propertyName);
    }
}
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Functional.EntityFramework;

public static class PropertyBuilderExtensions
{
    public static void HasOptionProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        string? backingFieldName = default,
        string? columnName = default) where TEntity : class
    {
        var type = typeof(TEntity);
        var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;

        // TODO: Convert `propertyName` into a camel-case
        var backingField = backingFieldName ?? propertyName.ToLowerInvariant();

        var key = (type, propertyName);

        var props = type.GetRuntimeProperties();
        var propertyInfo =
            props.SingleOrDefault(p => p.Name == backingField);

        EntityOptionMapping.OptionBackingFieldMapping.Add(
            key,
            propertyInfo ?? throw new Exception($"Unable to find property '{backingField}'"));

        builder.Property(backingField)
            .HasColumnName(columnName ?? backingField)
            .IsRequired(false);
    }
}
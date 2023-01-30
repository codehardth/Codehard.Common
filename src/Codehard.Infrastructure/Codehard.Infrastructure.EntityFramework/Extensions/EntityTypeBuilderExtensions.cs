using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Extensions;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Configure an enum properties on entity type by mapping to its corresponding string value.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="bindingAttr"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> MapEnumPropertiesToString<T>(
        this EntityTypeBuilder<T> builder,
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public)
        where T : class
    {
        var properties =
            typeof(T).GetProperties(bindingAttr)
                .Where(IsEnumOrNullableEnum);

        foreach (var property in properties)
        {
            builder.Property(property.Name)
                .HasConversion<string>();
        }

        return builder;

        static bool IsEnumOrNullableEnum(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            return type.IsEnum || Nullable.GetUnderlyingType(type) is { IsEnum: true };
        }
    }
}
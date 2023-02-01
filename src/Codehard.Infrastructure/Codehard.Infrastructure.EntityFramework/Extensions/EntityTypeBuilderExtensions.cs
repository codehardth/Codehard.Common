using System.Reflection;
using Codehard.Common.DomainModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codehard.Infrastructure.EntityFramework.Extensions;

public static class EntityTypeBuilderExtensions
{
    private static Dictionary<Type, WeakReference<PropertyInfo[]>> propertyInfoCache = new();

    /// <summary>
    /// Configure an enum properties on entity type by mapping to its corresponding string value.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> MapEnumPropertiesToString<T>(
        this EntityTypeBuilder<T> builder)
        where T : class
    {
        var properties =
            GetPropertyInfoFromCache<T>()
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

    /// <summary>
    /// Configure an EntityKey properties on entity type by mapping to its corresponding internal value.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> MapEntityKeyPropertiesToType<T>(
        this EntityTypeBuilder<T> builder)
        where T : class
    {
        var properties =
            GetPropertyInfoFromCache<T>();

        foreach (var property in properties)
        {
            switch (property.PropertyType)
            {
                case var x when x == typeof(IntegerKey):
                    builder.Property<IntegerKey>(property.Name)
                        .HasConversion(
                            i => i.Value,
                            i => new IntegerKey(i));
                    break;
                case var x when x == typeof(LongKey):
                    builder.Property<LongKey>(property.Name)
                        .HasConversion(
                            i => i.Value,
                            i => new LongKey(i));
                    break;
                case var x when x == typeof(StringKey):
                    builder.Property<StringKey>(property.Name)
                        .HasConversion(
                            i => i.Value,
                            i => new StringKey(i));
                    break;
                case var x when x == typeof(GuidKey):
                    builder.Property<GuidKey>(property.Name)
                        .HasConversion(
                            i => i.Value,
                            i => new GuidKey(i));
                    break;
                default:
                    continue;
            }
        }

        return builder;
    }

    private static PropertyInfo[] GetPropertyInfoFromCache<T>(
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public)
    {
        var type = typeof(T);

        if (propertyInfoCache.TryGetValue(type, out var wr1)
            && wr1.TryGetTarget(out var props))
        {
            return props;
        }

        var properties =
            typeof(T).GetProperties(bindingAttr).ToArray();

        var newWeakRef = new WeakReference<PropertyInfo[]>(properties, false);

        propertyInfoCache[type] = newWeakRef;

        return properties;
    }
}
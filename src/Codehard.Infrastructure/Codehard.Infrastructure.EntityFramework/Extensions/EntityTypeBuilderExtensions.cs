using System.Reflection;
using Codehard.Common.DomainModel;
using Codehard.Common.DomainModel.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    }

    /// <summary>
    /// Configure an enum properties on entity type by mapping to its corresponding string value.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TDependant"></typeparam>
    /// <returns></returns>
    public static OwnedNavigationBuilder<TOwner, TDependant> MapEnumPropertiesToString<TOwner, TDependant>(
        this OwnedNavigationBuilder<TOwner, TDependant> builder)
        where TDependant : class
        where TOwner : class
    {
        var properties =
            GetPropertyInfoFromCache<TDependant>()
                .Where(IsEnumOrNullableEnum);

        foreach (var property in properties)
        {
            builder.Property(property.Name)
                .HasConversion<string>();
        }

        return builder;
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
                            k => k.Value,
                            i => new IntegerKey(i));
                    break;
                case var x when x == typeof(LongKey):
                    builder.Property<LongKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            l => new LongKey(l));
                    break;
                case var x when x == typeof(StringKey):
                    builder.Property<StringKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            s => new StringKey(s));
                    break;
                case var x when x == typeof(GuidKey):
                    builder.Property<GuidKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            g => new GuidKey(g));
                    break;
                default:
                    continue;
            }
        }

        return builder;
    }

    /// <summary>
    /// Configure an EntityKey properties on entity type by mapping to its corresponding internal value.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static OwnedNavigationBuilder<TOwner, TDependant> MapEntityKeyPropertiesToType<TOwner, TDependant>(
        this OwnedNavigationBuilder<TOwner, TDependant> builder)
        where TDependant : class
        where TOwner : class
    {
        var properties =
            GetPropertyInfoFromCache<TDependant>();

        foreach (var property in properties)
        {
            switch (property.PropertyType)
            {
                case var x when x == typeof(IntegerKey):
                    builder.Property<IntegerKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            i => new IntegerKey(i));
                    break;
                case var x when x == typeof(LongKey):
                    builder.Property<LongKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            l => new LongKey(l));
                    break;
                case var x when x == typeof(StringKey):
                    builder.Property<StringKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            s => new StringKey(s));
                    break;
                case var x when x == typeof(GuidKey):
                    builder.Property<GuidKey>(property.Name)
                        .HasConversion(
                            k => k.Value,
                            g => new GuidKey(g));
                    break;
                default:
                    continue;
            }
        }

        return builder;
    }
    
    /// <summary>
    /// Configure a money properties on entity type by mapping to owned entity.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> MapMoneyPropertiesToOwnedEntity<T>(
        this EntityTypeBuilder<T> builder)
        where T : class
    {
        var properties =
            GetPropertyInfoFromCache<T>()
                .Where(IsMoneyType);

        foreach (var property in properties)
        {
            builder.OwnsOne<Money>(property.Name, buildAction =>
            {
                buildAction.Property(nameof(Money.Amount))
                    .IsRequired();
                buildAction.Property(nameof(Money.Currency))
                    .HasConversion(new EnumToStringConverter<Currency>())
                    .IsRequired();
            });
        }

        return builder;
    }
    
    /// <summary>
    /// Configure a money properties on entity type by mapping to owned entity.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TOwner"></typeparam>
    /// <typeparam name="TDependant"></typeparam>
    /// <returns></returns>
    public static OwnedNavigationBuilder<TOwner, TDependant> MapMoneyPropertiesToOwnedEntity<TOwner, TDependant>(
        this OwnedNavigationBuilder<TOwner, TDependant> builder)
        where TDependant : class
        where TOwner : class
    {
        var properties =
            GetPropertyInfoFromCache<TDependant>()
                .Where(IsMoneyType);

        foreach (var property in properties)
        {
            builder.OwnsOne<Money>(property.Name, buildAction =>
            {
                buildAction.Property(nameof(Money.Amount))
                    .IsRequired();
                buildAction.Property(nameof(Money.Currency))
                    .HasConversion(new EnumToStringConverter<Currency>())
                    .IsRequired();
            });
        }

        return builder;
    }

    private static bool IsMoneyType(PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;

        return type == typeof(Money);
    }

    private static bool IsEnumOrNullableEnum(PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;

        return type.IsEnum || Nullable.GetUnderlyingType(type) is { IsEnum: true };
    }

    private static IEnumerable<PropertyInfo> GetPropertyInfoFromCache<T>(
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public)
    {
        var type = typeof(T);

        if (propertyInfoCache.TryGetValue(type, out var wr)
            && wr.TryGetTarget(out var props))
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
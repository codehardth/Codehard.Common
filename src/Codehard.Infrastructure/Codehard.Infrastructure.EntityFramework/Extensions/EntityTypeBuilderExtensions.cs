using System.Reflection;
using Codehard.Common.DomainModel;
using Codehard.Common.DomainModel.Types;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Codehard.Infrastructure.EntityFramework.Extensions;

/// <summary>
/// Contains extension methods for <see cref="EntityTypeBuilder"/> and <see cref="OwnedNavigationBuilder"/> to simplify
/// entity configuration.
/// </summary>
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
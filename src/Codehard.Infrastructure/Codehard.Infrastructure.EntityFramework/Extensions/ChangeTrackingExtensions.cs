using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codehard.Infrastructure.EntityFramework.Extensions;

internal class CompositeEqualityComparer<T> :
    IEqualityComparer,
    IEqualityComparer<T>
{
    private readonly IReadOnlyCollection<Func<object, object?>> propertyGetters;

    public CompositeEqualityComparer(IReadOnlyCollection<Func<object, object?>> propertyGetters)
    {
        this.propertyGetters = propertyGetters;
    }

    public new bool Equals(object? x, object? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        var hashCodesEqual = true;
        foreach (var getter in propertyGetters)
        {
            var xValue = getter(x);
            var yValue = getter(y);

            if (xValue == null && yValue == null)
                continue;

            if (xValue == null || yValue == null)
            {
                hashCodesEqual = false;
                break;
            }

            if (!xValue.Equals(yValue))
            {
                hashCodesEqual = false;
                break;
            }
        }

        return hashCodesEqual;
    }

    public bool Equals(T? x, T? y)
    {
        return Equals(x, (object?)y);
    }

    public int GetHashCode(T? obj)
    {
        return GetHashCode((object?)obj);
    }

    public int GetHashCode(object? obj)
    {
        if (obj == null)
            return 0;

        var hashCode = 17;

        foreach (var getter in propertyGetters)
        {
            var value = getter(obj);
            hashCode = hashCode * 23 + (value?.GetHashCode() ?? 0);
        }

        return hashCode;
    }
}

/// <summary>
/// Provides extension methods for tracking changes in entity objects.
/// </summary>
public static class ChangeTrackingExtensions
{
    /// <summary>
    /// Updates an entity in the database if it has changed.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being tracked.</typeparam>
    /// <param name="context">The <see cref="DbContext"/> that contains the entity.</param>
    /// <param name="originalEntity">The original entity being tracked.</param>
    /// <param name="modifiedEntity">The modified entity.</param>
    /// <returns>The modified entity.</returns>
    public static TEntity? UpdateIfChanged<TEntity>(
        this DbContext context, TEntity? originalEntity, TEntity? modifiedEntity)
        where TEntity : class
    {
        if (originalEntity == null && modifiedEntity == null)
        {
            return null;
        }

        if (originalEntity == modifiedEntity)
        {
            return modifiedEntity;
        }

        if (originalEntity == null)
        {
            var addedEntityEntry = context.Entry(modifiedEntity!);

            addedEntityEntry.SetStateAsAdded();

            return modifiedEntity;
        }

        var entityEntry = context.Entry(originalEntity);

        entityEntry.UpdateIfChanged(modifiedEntity);

        return entityEntry.Entity;
    }

    private static void UpdateIfChanged(
        this EntityEntry originalEntityEntry, object? modifiedEntity)
    {
        if (originalEntityEntry.Entity == modifiedEntity)
        {
            return;
        }

        if (modifiedEntity == null)
        {
            originalEntityEntry.State = EntityState.Deleted;

            return;
        }

        originalEntityEntry.State = EntityState.Modified;

        originalEntityEntry.UpdateIfScalarPropertiesChanged(modifiedEntity);
        originalEntityEntry.UpdateIfNavigationPropertiesChanged(modifiedEntity);
    }

    private static void UpdateIfScalarPropertiesChanged(
        this EntityEntry originalEntityEntry, object modifiedEntity)
    {
        var scalarProperties = originalEntityEntry.Metadata.GetProperties().ToList();

        foreach (var property in scalarProperties)
        {
            if (property.PropertyInfo == null)
            {
                continue;
            }

            var originalValue = property.PropertyInfo.GetValue(originalEntityEntry.Entity);
            var modifiedValue = property.PropertyInfo.GetValue(modifiedEntity);

            if (Equals(originalValue, modifiedValue))
            {
                originalEntityEntry.Property(property.Name).IsModified = false;
                continue;
            }

            originalEntityEntry.Property(property.Name).CurrentValue = modifiedValue;
        }
    }

    private static void UpdateIfNavigationPropertiesChanged(
        this EntityEntry originalEntityEntry, object modifiedEntity)
    {
        var navigationProperties = originalEntityEntry.Metadata.GetNavigations().ToList();

        foreach (var property in navigationProperties)
        {
            if (property.PropertyInfo == null)
            {
                continue;
            }

            var modifiedValue = property.PropertyInfo.GetValue(modifiedEntity);

            if (property.IsCollection)
            {
                originalEntityEntry.Collection(property.Name)
                                   .UpdateIfChanged((IEnumerable?)modifiedValue);

                continue;
            }

            if (property.TargetEntityType.IsOwned())
            {
                if (originalEntityEntry.Reference(property.Name).CurrentValue == null
                    && modifiedValue != null)
                {
                    originalEntityEntry.Reference(property.Name).CurrentValue = modifiedValue;

                    continue;
                }

                originalEntityEntry.Reference(property.Name).TargetEntry
                                   ?.UpdateIfChanged(modifiedValue);

                continue;
            }

            originalEntityEntry.Navigation(property.Name).CurrentValue = modifiedValue;
        }
    }

    private static void UpdateIfChanged(
        this CollectionEntry collectionEntry, IEnumerable? modifiedCollection)
    {
        var collectionType = collectionEntry.Metadata.ClrType;
        var elementType = collectionType.GetGenericArguments()[0];

        IEnumerable originalCollection = 
            collectionEntry.CurrentValue?.Cast<object>().ToArray()
            ?? Array.CreateInstance(elementType, 0);
        
        originalCollection = CastToIEnumerable(originalCollection);
        modifiedCollection ??= Array.CreateInstance(elementType, 0);

        // Invoke the SequenceEqual method with the original and modified collections
        if (SequenceEqual(originalCollection, modifiedCollection))
        {
            return;
        }
        
        var primaryKeyComparer = CreatePrimaryKeyComparer();

        // Set new item states to Added
        var itemsToAdd = Except(
            modifiedCollection, originalCollection, primaryKeyComparer);
        
        var itemsToRemove = Except(
            originalCollection, modifiedCollection, primaryKeyComparer);
        
        var itemsToModify = Except(
            originalCollection, itemsToRemove, primaryKeyComparer);

        SetItemAsAdded(itemsToAdd);
        SetItemAsDeleted(itemsToRemove);
        SetItemAsModified(itemsToModify);
        
        return;

        IEnumerable CastToIEnumerable(object? collection)
        {
            // Get the Cast<T>() method from the Enumerable class using reflection
            var castMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.Cast))!
                .MakeGenericMethod(elementType);

            // Invoke the Cast<T>() method with the original and modified collections
            return (IEnumerable)castMethod.Invoke(
                null, new[] { collection })!;
        }
        
        bool SequenceEqual(IEnumerable collection, IEnumerable? otherCollection)
        {
            // Get the SequenceEqual<T>() method from the Enumerable class using reflection
            var sequenceEqualMethod = typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m =>
                    m.Name == nameof(Enumerable.SequenceEqual) &&
                    m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType);

            // Invoke the SequenceEqual<T>() method with the original and modified collections
            return (bool)sequenceEqualMethod.Invoke(
                null, 
                new[] { (object)collection, otherCollection })!;
        }

        IEqualityComparer? CreatePrimaryKeyComparer()
        {
            // Get the type of the CompositeEqualityComparer<T> class
            var compositeEqualityComparerType = typeof(CompositeEqualityComparer<>);

            // Construct a type argument for the CompositeEqualityComparer<T> class
            var genericCompositeEqualityComparerType =
                compositeEqualityComparerType.MakeGenericType(elementType);

            // Get the constructor for the CompositeEqualityComparer<T> class that takes an IReadOnlyCollection<Func<object, object?>> parameter
            var constructor = genericCompositeEqualityComparerType.GetConstructor(
                new[] { typeof(IReadOnlyCollection<Func<object, object?>>) });

            var primaryKeyProperties =
                collectionEntry.Metadata.TargetEntityType.FindPrimaryKey()
                    ?.Properties;

            var propertyGetters =
                primaryKeyProperties
                    ?.Select(
                        p => 
                        (Func<object, object?>)(entity => p.PropertyInfo!.GetValue(entity)))
                    .ToArray();
            
            // Call the constructor to create an instance of the CompositeEqualityComparer<T> class
            var compositeEqualityComparerInstance =
                constructor?.Invoke(new object?[] { propertyGetters });

            return (IEqualityComparer?)compositeEqualityComparerInstance;
        }
        
        IEnumerable Except(object? collection, object? otherCollection, object? comparer)
        {
            // Get the Except<T>() method from the Enumerable class using reflection
            var exceptMethod = typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(Enumerable.Except) && m.GetParameters().Length == 3)
                .MakeGenericMethod(elementType);

            // Invoke the Except<T>() method with the original and modified collections
            return (IEnumerable)exceptMethod.Invoke(
                        null,
                        new[] { collection, otherCollection, comparer })!;
        }
        
        void SetItemAsAdded(IEnumerable items)
        {
            if (collectionType.IsGenericType 
                && collectionType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var listType = typeof(List<>).MakeGenericType(elementType);

                // Get the MethodInfo object for the AddRange method
                var addRangeMethod = listType.GetMethod(nameof(List<object>.AddRange));

                // Call the AddRange method on the list object
                addRangeMethod!.Invoke(
                    collectionEntry.CurrentValue,
                    new object[] { items });
                
                foreach (var item in items)
                {
                    var itemEntry = collectionEntry.FindEntry(item);
                
                    itemEntry!.State = EntityState.Added;
                }
            }
        }

        void SetItemAsDeleted(IEnumerable items)
        {
            // Find items to remove
            foreach (var entityEntry 
                     in items
                        .Cast<object>()
                        .Select(collectionEntry.FindEntry)
                        .Where(entityEntry => entityEntry != null))
            {
                entityEntry!.State = EntityState.Deleted;
            }
        }

        void SetItemAsModified(IEnumerable items)
        {
            // Find items to update
            foreach (var originalItem in items)
            {
                var newItem =
                    modifiedCollection
                        .Cast<object>()
                        .FirstOrDefault(
                            modifiedEntity => 
                                primaryKeyComparer.Equals(originalItem, modifiedEntity));

                if (originalItem.Equals(newItem))
                {
                    continue;
                }

                var itemEntry = collectionEntry.FindEntry(originalItem);

                itemEntry?.UpdateIfChanged(newItem);
            }
        }
    }

    private static void SetStateAsAdded(this EntityEntry entityEntry)
    {
        entityEntry.State = EntityState.Added;

        var entityType = entityEntry.Metadata;

        var navigationProperties = entityType.GetNavigations().ToList();

        foreach (var property in navigationProperties
                     .Where(p => !p.IsShadowProperty()
                                 && p.PropertyInfo != null
                                 && p.TargetEntityType.IsOwned()))
        {
            var propertyValue = entityEntry.Navigation(property.Name).CurrentValue;

            if (property.IsCollection
                && property.ClrType.GetGenericArguments().Any())
            {
                var collection = 
                    propertyValue as IEnumerable
                    ?? Array.CreateInstance(property.ClrType.GetGenericArguments()[0], 0);

                foreach (var item in collection)
                {
                    var itemEntry = entityEntry.Collection(property.Name).FindEntry(item);

                    itemEntry?.SetStateAsAdded();
                }

                continue;
            }

            var targetEntry = entityEntry.Reference(property.Name).TargetEntry;

            targetEntry?.SetStateAsAdded();
        }
    }
}
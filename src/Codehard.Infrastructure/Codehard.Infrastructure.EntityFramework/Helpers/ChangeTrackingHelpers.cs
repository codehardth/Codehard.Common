using System.Reflection;
using Codehard.Infrastructure.EntityFramework.Extensions;
using static Codehard.Infrastructure.EntityFramework.Helpers.MemoizationHelpers;

namespace Codehard.Infrastructure.EntityFramework.Helpers;

internal static class ChangeTrackingHelpers
{
    internal static MethodInfo GetCastMethod(Type elementType)
        => Memo<Type, MethodInfo>(type =>
            typeof(Enumerable)
                .GetMethod(nameof(Enumerable.Cast))!
                .MakeGenericMethod(type))(elementType);

    internal static MethodInfo GetSeqEqualMethod(Type elementType)
        => Memo<Type, MethodInfo>(type =>
            typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m =>
                    m.Name == nameof(Enumerable.SequenceEqual) &&
                    m.GetParameters().Length == 2)
                .MakeGenericMethod(type))(elementType);

    internal static ConstructorInfo GetConstructorInfo(Type elementType)
        => Memo<Type, ConstructorInfo>(type =>
        {
            // Get the type of the CompositeEqualityComparer<T> class
            var compositeEqualityComparerType = typeof(CompositeEqualityComparer<>);

            // Construct a type argument for the CompositeEqualityComparer<T> class
            var genericCompositeEqualityComparerType =
                compositeEqualityComparerType.MakeGenericType(type);

            // Get the constructor for the CompositeEqualityComparer<T> class that takes an IReadOnlyCollection<Func<object, object?>> parameter
            var constructorInfo = genericCompositeEqualityComparerType.GetConstructor(
                new[] { typeof(IReadOnlyCollection<Func<object, object?>>) });

            return constructorInfo ??
                   throw new Exception(
                       $"Unable to find suitable constructor for {genericCompositeEqualityComparerType}");
        })(elementType);

    internal static MethodInfo GetExceptMethod(Type elementType)
        => Memo<Type, MethodInfo>(type =>
            typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(Enumerable.Except) && m.GetParameters().Length == 3)
                .MakeGenericMethod(type))(elementType);

    internal static MethodInfo GetAddRangeMethod(Type elementType)
        => Memo<Type, MethodInfo>(type =>
        {
            var listType = typeof(List<>).MakeGenericType(type);

            // Get the MethodInfo object for the AddRange method
            var addRangeMethod = listType.GetMethod(nameof(List<object>.AddRange));

            return addRangeMethod!;
        })(elementType);
}
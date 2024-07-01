using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework.Extensions;

/// <summary>
/// Provides extension methods for the ModelBuilder class.
/// These methods allow for applying entity type configurations from a specified assembly to the model.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies all entity type configurations defined in the specified assembly to the model.
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext that the configurations apply to.</typeparam>
    /// <param name="modelBuilder">The model builder to apply the configurations to.</param>
    /// <param name="assembly">The assembly containing the entity type configurations.</param>
    /// <returns>The model builder with the configurations applied.</returns>
    public static ModelBuilder ApplyConfigurationsFromAssemblyFor<TContext>(this ModelBuilder modelBuilder, Assembly assembly)
        where TContext : DbContext
    {
        foreach (var configurationType in GetConfigurationsWithModelBuilderInjected(assembly, typeof(TContext)))
        {
            // A workaround since 'ApplyConfiguration' requires
            // a parameter to be IEntityTypeConfiguration<TEntity>
            // but we can't resolve TEntity at runtime
            dynamic? configuration = Activator.CreateInstance(configurationType, modelBuilder);
            modelBuilder.ApplyConfiguration(configuration);
        }

        return modelBuilder.ApplyConfigurationsFromAssembly(assembly,
            type => IsCompatible(type.BaseType, typeof(TContext)));
    }

    private static IEnumerable<Type> GetConfigurationsWithModelBuilderInjected(Assembly assembly, Type contextType)
    {
        var configurations =
            assembly.GetTypes()
                    .Where(type =>
                    {
                        var isCompatible = IsCompatible(type.BaseType, contextType);

                        var ctors = type.GetConstructors();
                        var hasModelBuilderInjectedCtor = ctors.Any(ci =>
                        {
                            var parameters = ci.GetParameters();

                            return parameters.Length == 1 &&
                                   parameters[0].ParameterType == typeof(ModelBuilder);
                        });

                        return isCompatible && hasModelBuilderInjectedCtor;
                    });

        return configurations;
    }

    private static bool IsCompatible(Type? configurationType, Type contextType)
    {
        if (configurationType == null)
        {
            return false;
        }

        var isCompatible =
            configurationType is { IsGenericType: true } &&
            configurationType.GetGenericTypeDefinition() == typeof(EntityTypeConfigurationBase<,>) &&
            configurationType.GenericTypeArguments.Length == 2 &&
            configurationType.GenericTypeArguments[1] == contextType;

        return isCompatible;
    }
}
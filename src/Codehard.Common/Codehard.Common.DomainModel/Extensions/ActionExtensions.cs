using System.Runtime.CompilerServices;

namespace Codehard.Common.DomainModel.Extensions;

/// <summary>
/// Provides extension methods for actions.
/// </summary>
public static class ActionExtensions
{
    /// <summary>
    /// Loads a navigation property of a related entity using the specified loader action.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    /// <param name="loader">The loader action used to load the related entity.</param>
    /// <param name="entity">The entity that owns the navigation property.</param>
    /// <param name="navigationField">The reference to the related entity.</param>
    /// <param name="navigationName">The name of the navigation property (optional).</param>
    /// <returns>The loaded related entity.</returns>
    /// <remarks>
    /// This extension method is typically used in conjunction with lazy loading.
    /// It invokes the specified loader action and returns the reference to the related entity.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when the loader action is null.</exception>
    public static TRelated Load<TRelated>(
        this Action<object, string> loader,
        object entity,
        ref TRelated navigationField,
        [CallerMemberName] string navigationName = null!)
        where TRelated : class
    {
        loader?.Invoke(entity, navigationName);

        return navigationField;
    }
}
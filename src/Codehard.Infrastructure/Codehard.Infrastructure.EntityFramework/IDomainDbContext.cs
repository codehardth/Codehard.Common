using Codehard.Common.DomainModel;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// Interface for a domain event-aware database context, providing a method to asynchronously publish domain events.
/// </summary>
public interface IDomainEventDbContext
{
    /// <summary>
    /// Asynchronous method to publish a domain event.
    /// Derived classes should implement this method to handle the actual publishing of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to be published.</param>
    /// <returns>An asynchronous task representing the operation.</returns>
    protected internal Task PublishDomainEventAsync(IDomainEvent domainEvent);
}
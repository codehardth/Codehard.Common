using Codehard.Infrastructure.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Codehard.Infrastructure.EntityFramework;

/// <summary>
/// Extension methods for <see cref="DbContextOptionsBuilder"/> to register a domain event publisher interceptor.
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Registers a domain event publisher interceptor for the <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to which the interceptor is added.</param>
    /// <param name="delegate">The delegate responsible for asynchronously publishing domain events (optional).</param>
    public static DbContextOptionsBuilder AddDomainEventPublisherInterceptor(
        this DbContextOptionsBuilder optionsBuilder,
        PublishDomainEventDelegate? @delegate = default)
    {
        return optionsBuilder.AddInterceptors(new DomainEventPublisherInterceptor(@delegate));
    }
}
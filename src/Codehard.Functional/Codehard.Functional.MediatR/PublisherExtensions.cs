using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

/// <summary>
/// Provides extension methods for the <see cref="IPublisher"/> interface.
/// </summary>
public static class PublisherExtensions
{
    /// <summary>
    /// Publishes a notification asynchronously within an Eff monad.
    /// </summary>
    /// <typeparam name="TNotification">The type of the notification.</typeparam>
    /// <param name="publisher">The publisher to use for publishing the notification.</param>
    /// <param name="notification">The notification to publish.</param>
    /// <returns>An <see cref="LanguageExt.Eff{Unit}"/> representing the asynchronous operation.</returns>
    public static Eff<LanguageExt.Unit> PublishEff<TNotification>(
        this IPublisher publisher,
        TNotification notification)
        where TNotification : INotification
    {
        return
            liftIO(async env =>
            {
                await publisher.Publish(notification, env.Token);
                return unit;
            });
    }
}
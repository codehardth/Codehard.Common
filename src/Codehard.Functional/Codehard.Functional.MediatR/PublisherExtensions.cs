using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class PublisherExtensions
{
    /// <summary>
    /// Publishes a notification
    /// </summary>
    public static Eff<LanguageExt.Unit> PublishAff<TNotification>(
        this IPublisher publisher,
        TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        return
            liftEff(async () =>
            {
                await publisher.Publish(notification, cancellationToken);
                return unit;
            });
    }
}
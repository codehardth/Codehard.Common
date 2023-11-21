using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace MediatR;

public static class PublisherExtensions
{
    public static Aff<LanguageExt.Unit> PublishAff<TNotification>(
        this IPublisher publisher,
        TNotification notification)
        where TNotification : INotification
    {
        return
            Aff(async () =>
            {
                await publisher.Publish(notification);
                return unit;
            });
    }
}
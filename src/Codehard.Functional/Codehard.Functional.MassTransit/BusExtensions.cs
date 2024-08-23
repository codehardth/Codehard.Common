// ReSharper disable once CheckNamespace
namespace MassTransit;

/// <summary>
/// The Collection of Masstransit bus extension
/// </summary>
public static class BusExtensions
{
    /// <summary>
    /// Publish message as an Async Effect
    /// </summary>
    public static Eff<Unit> PublishAsAff<T>(this IBus bus, T message)
        where T : class
    {
        return liftEff(() => bus.Publish(message).ToUnit());
    }
}
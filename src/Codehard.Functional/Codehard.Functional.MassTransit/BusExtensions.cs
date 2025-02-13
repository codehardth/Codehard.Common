// ReSharper disable once CheckNamespace
namespace MassTransit;

/// <summary>
/// The Collection of Masstransit bus extension
/// </summary>
public static class BusExtensions
{
    /// <summary>
    /// Publish message as an Effect
    /// </summary>
    public static Eff<Unit> PublishAsEff<T>(this IBus bus, T message)
        where T : class
    {
        return liftEff(() => bus.Publish(message).ToUnit());
    }
}
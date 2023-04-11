namespace MassTransit;

/// <summary>
/// The Collection of Masstransit bus extension
/// </summary>
public static class BusExtensions
{
    /// <summary>
    /// Publish message as a Async Effect
    /// </summary>
    public static Aff<Unit> PublishAsAff<T>(this IBus bus, T message)
        where T : class
    {
        return Aff(async () => await bus.Publish(message).ToUnit());
    }
}
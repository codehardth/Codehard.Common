using Codehard.Common.DomainModel.Extensions;

namespace Codehard.Common.DomainModel;

public abstract class Entity<T>
    : IEntity<T> where T : struct
{
    private readonly Action<object, string> lazyLoader;

    protected Entity(Action<object, string> lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    public abstract T Id { get; }

    protected TRelated LoadNavigationProperty<TRelated>(TRelated refInstance)
        where TRelated : class
        => this.lazyLoader.Load(this, ref refInstance);

    protected IReadOnlyCollection<TRelated> LoadNavigationPropertyCollection<TRelated>(List<TRelated> collection)
        => this.lazyLoader.Load(this, ref collection);
}
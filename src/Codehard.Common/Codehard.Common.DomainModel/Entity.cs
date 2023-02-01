using System.Runtime.CompilerServices;
using Codehard.Common.DomainModel.Extensions;

namespace Codehard.Common.DomainModel;

public abstract class Entity<TKey>
    : IEntity<TKey> where TKey : IEntityKey
{
    private readonly Action<object, string> lazyLoader;

    protected Entity()
    {
    }

    protected Entity(Action<object, string> lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    public abstract TKey Id { get; protected init; }

    protected TRelated LoadNavigationProperty<TRelated>(
        TRelated refInstance,
        [CallerMemberName] string navigationName = default!)
        where TRelated : class
        => this.lazyLoader.Load(this, ref refInstance, navigationName);

    protected IReadOnlyCollection<TRelated> LoadNavigationPropertyCollection<TRelated>(
        List<TRelated> collection,
        [CallerMemberName] string navigationName = default!)
        => this.lazyLoader.Load(this, ref collection, navigationName);

    public override bool Equals(object? o)
    {
        return o is Entity<TKey> e && e.Id.Equals(this.Id);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}
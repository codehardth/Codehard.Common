using Codehard.Common.DomainModel;
using LanguageExt;

namespace Infrastructure.Test.Entities;

public class MyModel : Entity<GuidKey>
{
    public MyModel()
    {
    }

    private MyModel(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    private List<ChildModel> childs = new();

    public override GuidKey Id { get; protected init; }

    public string Value { get; init; }

    public Option<int> NullableValue { get; init; }

    public virtual IReadOnlyCollection<ChildModel> Childs
        => this.LoadNavigationPropertyCollection(this.childs);

    public void AddChild(string id)
    {
        this.childs.Add(ChildModel.Create(id));
    }

    public static MyModel Create()
    {
        return new MyModel
        {
            Id = new GuidKey(Guid.NewGuid()),
            Value = "Hello World",
            NullableValue = Optional(10),
        };
    }
}
using Codehard.Common.DomainModel;
using LanguageExt;

namespace Infrastructure.Test.Entities;

public class MyModel : Entity<GuidKey>
{
    private int? _number { get; set; }

    private string? _text { get; set; }

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

    public Option<int> Number
    {
        get => Optional(this._number);
        set => this._number = value.MatchUnsafe(x => x, () => (int?)default);
    }

    public Option<string> Text
    {
        get => Optional(this._text);
        set => this._text = value.MatchUnsafe(identity, () => default);
    }

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
            Number = 10,
        };
    }
}
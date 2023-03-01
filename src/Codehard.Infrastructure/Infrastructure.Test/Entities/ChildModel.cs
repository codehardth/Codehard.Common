using Codehard.Common.DomainModel;
using LanguageExt;

namespace Infrastructure.Test.Entities;

public class ChildModel : Entity<StringKey>
{
    public ChildModel()
    {
    }

    private ChildModel(Action<object, string> lazyLoader)
        : base(lazyLoader)
    {
    }

    public override StringKey Id { get; protected init; }

    private decimal? valueOpt;

    public Option<decimal> ValueOpt { get; set; }

    public static ChildModel Create(string id)
    {
        return new ChildModel
        {
            Id = new StringKey(id),
        };
    }
}
using System.Linq.Expressions;
using Codehard.Common.DomainModel;
using Codehard.Common.DomainModel.Attributes;
using Vogen;

public delegate bool IsCreatedBeforeSpecificYearDelegate(MyEntityRoot entity, int value);

public delegate bool TestBoolDelegate(MyEntityRoot entity);

public delegate string TestStringDelegate(MyEntityRoot entity);

public delegate bool NoArgumentDelegate();

[ValueObject(typeof(Guid), conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public partial class MyEntityKey : IEntityKey
{
}

public partial record MyEntityRoot : IAggregateRoot<MyEntityKey>, IDisposable
{
    public MyEntityKey Id { get; }

    public DateTimeOffset CreatedAt { get; }

    [Specification]
    public Func<Guid, Expression<Func<MyEntityRoot, bool>>> FuncWithArgWhichReturnsExpression = id => e => e.Id == id;

    [Specification] 
    public Expression<Func<MyEntityRoot, bool>> Expression = e => e.Id == Guid.Empty;

    [Specification] 
    public Func<MyEntityRoot, int, int, bool> FuncWithMultipleArgsWhichReturnsBool = (e, x, y) => x > y;

    [Specification]
    public Func<Guid, int, string, Expression<Func<MyEntityRoot, bool>>> FuncWithMultipleArgsWhichReturnsExpression =
        (a, b, c) => e => e.Id == a;

    [Specification]
    public IsCreatedBeforeSpecificYearDelegate Delegate1 = (entity, year) => entity.CreatedAt.Year < year;

    [Specification] 
    public TestBoolDelegate Delegate2 = e => true;

    [Specification] 
    public TestBoolDelegate Delegate3 = e => e.CreatedAt == DateTimeOffset.UtcNow;

    [Specification] 
    public TestStringDelegate Delegate4 = e => e.ToString();

    [Specification] 
    public NoArgumentDelegate Delegate5 = () => true;

    public void Dispose()
    {
    }
}
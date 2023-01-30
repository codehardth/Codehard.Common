namespace Codehard.Functional;

public static class TaskExtensions
{
    public static Aff<Option<A>> ToAffOption<A>(this Task<A?> ma)
        => ma.Map(Optional).ToAff();

    public static Aff<Option<A>> ToAffOption<A>(this ValueTask<A?> ma)
        => ma.Map(Optional).ToAff();
    
    public static Aff<Unit> ToAffUnit(this ValueTask ma)
        => ma.ToUnit().ToAff();
    
    public static Aff<Unit> ToAffUnit(this Task ma)
        => ma.ToUnit().ToAff();
}
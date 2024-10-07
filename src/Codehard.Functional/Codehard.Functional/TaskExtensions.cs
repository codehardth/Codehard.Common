// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks;

/// <summary>
/// Extension methods for working with Task and ValueTask in Eff monad.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Converts a Task of nullable value type to an Eff&lt;Option&gt; monad.
    /// </summary>
    /// <typeparam name="A">The type of the value (nullable) to convert.</typeparam>
    /// <param name="ma">The Task representing the nullable value.</param>
    /// <returns>An Eff&lt;Option&gt; monad containing the result of the Task wrapped in Option.</returns>
    public static Eff<Option<A>> ToEffOption<A>(this Task<A?> ma)
        => liftEff(() => ma.Map(Optional));

    /// <summary>
    /// Converts a ValueTask of nullable value type to an Eff&lt;Option&gt; monad.
    /// </summary>
    /// <typeparam name="A">The type of the value (nullable) to convert.</typeparam>
    /// <param name="ma">The ValueTask representing the nullable value.</param>
    /// <returns>An Eff&lt;Option&gt; monad containing the result of the ValueTask wrapped in Option.</returns>
    public static Eff<Option<A>> ToEffOption<A>(this ValueTask<A?> ma)
        => liftEff(async () => await ma.Map(Optional));
    
    /// <summary>
    /// Converts a ValueTask of no returned result to an Eff&lt;Unit&gt; monad.
    /// </summary>
    /// <param name="ma">The ValueTask representing the effect with no returned result.</param>
    /// <returns>An Eff&lt;Unit&gt; monad representing the effect with no returned result.</returns>
    public static Eff<Unit> ToEffUnit(this ValueTask ma)
        => liftEff(async () => await ma.ToUnit());
    
    /// <summary>
    /// Converts a Task of no returned result to an Eff&lt;Unit&gt; monad.
    /// </summary>
    /// <param name="ma">The Task representing the effect with no returned result.</param>
    /// <returns>An Eff&lt;Unit&gt; monad representing the effect with no returned result.</returns>
    public static Eff<Unit> ToEffUnit(this Task ma)
        => liftEff(async () => await ma.ToUnit());
    
    /// <summary>
    /// Runs multiple Tasks in parallel and returns a single Task representing their completion with no result.
    /// </summary>
    /// <param name="tasks">The IEnumerable collection of Tasks to run in parallel.</param>
    /// <returns>A Task representing the completion of all the input Tasks with no result.</returns>
    public static Task<Unit> IterParallel(this IEnumerable<Task> tasks)
        => Task.WhenAll(tasks).ToUnit();
    
    /// <summary>
    /// Runs multiple Tasks in parallel within an Eff monad and returns an Eff&lt;Unit&gt; monad representing their completion with no result.
    /// </summary>
    /// <param name="tasks">The IEnumerable collection of Tasks to run in parallel.</param>
    /// <returns>An Eff&lt;Unit&gt; monad representing the completion of all the input Tasks with no result.</returns>
    public static Eff<Unit> IterParallelEff(this IEnumerable<Task> tasks)
        => liftEff(() => Task.WhenAll(tasks).ToUnit());
}
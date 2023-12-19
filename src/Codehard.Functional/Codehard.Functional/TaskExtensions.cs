// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks;

/// <summary>
/// Extension methods for working with Task and ValueTask in Aff monad.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Converts a Task of nullable value type to an Aff&lt;Option&gt; monad.
    /// </summary>
    /// <typeparam name="A">The type of the value (nullable) to convert.</typeparam>
    /// <param name="ma">The Task representing the nullable value.</param>
    /// <returns>An Aff&lt;Option&gt; monad containing the result of the Task wrapped in Option.</returns>
    public static Aff<Option<A>> ToAffOption<A>(this Task<A?> ma)
        => ma.Map(Optional).ToAff();

    /// <summary>
    /// Converts a ValueTask of nullable value type to an Aff&lt;Option&gt; monad.
    /// </summary>
    /// <typeparam name="A">The type of the value (nullable) to convert.</typeparam>
    /// <param name="ma">The ValueTask representing the nullable value.</param>
    /// <returns>An Aff&lt;Option&gt; monad containing the result of the ValueTask wrapped in Option.</returns>
    public static Aff<Option<A>> ToAffOption<A>(this ValueTask<A?> ma)
        => ma.Map(Optional).ToAff();
    
    /// <summary>
    /// Converts a ValueTask of no returned result to an Aff&lt;Unit&gt; monad.
    /// </summary>
    /// <param name="ma">The ValueTask representing the effect with no returned result.</param>
    /// <returns>An Aff&lt;Unit&gt; monad representing the effect with no returned result.</returns>
    public static Aff<Unit> ToAffUnit(this ValueTask ma)
        => ma.ToUnit().ToAff();
    
    /// <summary>
    /// Converts a Task of no returned result to an Aff&lt;Unit&gt; monad.
    /// </summary>
    /// <param name="ma">The Task representing the effect with no returned result.</param>
    /// <returns>An Aff&lt;Unit&gt; monad representing the effect with no returned result.</returns>
    public static Aff<Unit> ToAffUnit(this Task ma)
        => ma.ToUnit().ToAff();
    
    /// <summary>
    /// Runs multiple Tasks in parallel and returns a single Task representing their completion with no result.
    /// </summary>
    /// <param name="tasks">The IEnumerable collection of Tasks to run in parallel.</param>
    /// <returns>A Task representing the completion of all the input Tasks with no result.</returns>
    public static Task<Unit> IterParallel(this IEnumerable<Task> tasks)
        => Task.WhenAll(tasks).ToUnit();
    
    /// <summary>
    /// Runs multiple Tasks in parallel within an Aff monad and returns an Aff&lt;Unit&gt; monad representing their completion with no result.
    /// </summary>
    /// <param name="tasks">The IEnumerable collection of Tasks to run in parallel.</param>
    /// <returns>An Aff&lt;Unit&gt; monad representing the completion of all the input Tasks with no result.</returns>
    public static Aff<Unit> IterParallelAff(this IEnumerable<Task> tasks)
        => IterParallel(tasks).ToAff();
}
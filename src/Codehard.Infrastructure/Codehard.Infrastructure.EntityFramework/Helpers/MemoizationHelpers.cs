using System.Collections.Concurrent;

namespace Codehard.Infrastructure.EntityFramework.Helpers;

internal static class MemoizationHelpers
{
    public static Func<A> Memo<A>(Func<A> func)
    {
        var sync = new object();
        var value = default(A);
        var valueSet = false;

        return () =>
        {
            if (valueSet)
            {
                return value;
            }

            lock (sync)
            {
                if (!valueSet)
                {
                    value = func();
                    valueSet = true;
                }
            }

            return value;
        };
    }

    internal static Func<A, B> Memo<A, B>(Func<A, B> func)
        where B : class
    {
        var cache = new ConcurrentDictionary<A, WeakReference<B>>();
        return input =>
        {
            if (cache.TryGetValue(input, out var resultRef) &&
                resultRef.TryGetTarget(out var result))
            {
                return result;
            }

            var computed = func(input);
            cache[input] = new WeakReference<B>(computed, false);
            return computed;
        };
    }
}
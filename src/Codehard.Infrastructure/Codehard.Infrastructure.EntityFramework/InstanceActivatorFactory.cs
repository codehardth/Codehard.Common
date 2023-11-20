using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Codehard.Infrastructure.EntityFramework;

internal class InstanceActivatorFactory
{
    private readonly ConcurrentDictionary<Type, Func<object>> cache = new();

    private static InstanceActivatorFactory? instance;

    public static readonly InstanceActivatorFactory Instance = instance ??= new InstanceActivatorFactory();

    private InstanceActivatorFactory()
    {
    }

    public object CreateInstance(Type type)
    {
        if (this.cache.TryGetValue(type, out var activator))
        {
            return activator();
        }

        var expression = Expression.Lambda<Func<object>>(Expression.New(type));
        var compiledExpression = expression.Compile();

        this.cache.AddOrUpdate(type, compiledExpression, (_, _) => compiledExpression);

        return compiledExpression();
    }

    public object CreateInstance<T>()
    {
        return this.CreateInstance(typeof(T));
    }
}
using System.Collections.Immutable;

namespace Codehard.Functional;

public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="dictionary">Source dictionary.</param>
    /// <param name="key">The key whose value to get.</param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>When this method returns,
    /// the monad Option of value associated with the specified key, if the key is found;
    /// otherwise, the Option.None for the type of the value parameter.
    /// This parameter is passed uninitialized.</returns>
    public static Option<TValue> GetValueOption<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        => dictionary.TryGetValue(key, out var value) ? value : None;

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="dictionary">Source immutable dictionary.</param>
    /// <param name="key">The key whose value to get.</param>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns>When this method returns,
    /// the monad Option of value associated with the specified key, if the key is found;
    /// otherwise, the Option.None for the type of the value parameter.
    /// This parameter is passed uninitialized.</returns>
    public static Option<TValue> GetValueOption<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary,
        TKey key)
        => dictionary.TryGetValue(key, out var value) ? value : None;
}
using System.Diagnostics.Contracts;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Provides extension methods for a string.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the original string if it's not null or empty, otherwise returns the alternative value.
    /// </summary>
    [Pure]
    public static string IfNullOrEmpty(this string s, string alternative)
        => string.IsNullOrEmpty(s) ? alternative : s;

    /// <summary>
    /// Returns the original string if it's not null or whitespace, otherwise returns the alternative value.
    /// </summary>
    [Pure]
    public static string IfNullOrWhitespace(this string s, string alternative)
        => string.IsNullOrWhiteSpace(s) ? alternative : s;
}
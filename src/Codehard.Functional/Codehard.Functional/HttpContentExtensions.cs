#pragma warning disable CS1591

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace System.Net.Http;

/// <summary>
/// Http content extensions.
/// </summary>
public static class FunctionalHttpContentExtensions
{
    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner.
    /// with default serializer options that ignores property name casing.
    /// </summary>
    /// <returns>Returns an instance of <see cref="Option{A}"/></returns>
    /// <exception cref="JsonException">The JSON is invalid. -or- T is not compatible with the JSON. -or- There is remaining data in the stream.</exception>
    /// <exception cref="ArgumentNullException">There is no compatible System.Text.Json.Serialization.JsonConverter for T or its serializable members.</exception>
    /// <exception cref="NotSupportedException">utf8Json is null</exception>
    public static Task<Option<T>> ReadAsOptionalObjectAsync<T>(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        return ReadAsOptionalObjectAsync<T>(
            httpContent,
            HttpContentExtensions.CaseInsensitiveOptions,
            cancellationToken);
    }
    
    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner.
    /// </summary>
    /// <returns>Returns an instance of <see cref="Option{T}"/></returns>
    /// <exception cref="JsonException">The JSON is invalid. -or- T is not compatible with the JSON. -or- There is remaining data in the stream.</exception>
    /// <exception cref="ArgumentNullException">There is no compatible System.Text.Json.Serialization.JsonConverter for T or its serializable members.</exception>
    /// <exception cref="NotSupportedException">utf8Json is null</exception>
    public static Task<Option<T>> ReadAsOptionalObjectAsync<T>(
        this HttpContent httpContent,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default)
    {
        return
            httpContent
                .ReadAsObjectAsync<T>(options, cancellationToken)
                .Map(Optional);
    }
    
    public static Aff<Option<T>> ReadAsOptionalObjectAff<T>(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        return Aff(async () =>
            await httpContent.ReadAsOptionalObjectAsync<T>(
                HttpContentExtensions.CaseInsensitiveOptions,
                cancellationToken));
    }
}
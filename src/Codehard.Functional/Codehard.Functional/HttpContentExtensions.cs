using System.Text.Json;

namespace Codehard.Functional;

/// <summary>
/// Http content extensions.
/// </summary>
public static class HttpContentExtensions
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
            Common.Extensions.HttpContentExtensions.CaseInsensitiveOptions,
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
            Common.Extensions.HttpContentExtensions
                  .ReadAsObjectAsync<T>(httpContent, options, cancellationToken)
                  .Map(Optional);
    }
}
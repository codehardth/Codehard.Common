using System.Text.Json;

// ReSharper disable once CheckNamespace
namespace System.Net.Http;

/// <summary>
/// Provides extension methods for HTTP content.
/// </summary>
public static class HttpContentExtensions
{
    public static readonly JsonSerializerOptions CaseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner,
    /// with default serializer options that ignores property name casing.
    /// </summary>
    /// <param name="httpContent">The HTTP content to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type of the POCO to deserialize the JSON into.</typeparam>
    /// <returns>Returns an instance of <typeparamref name="T"/> or null if the stream is empty.</returns>
    /// <exception cref="JsonException">The JSON is invalid. -or- T is not compatible with the JSON. -or- There is remaining data in the stream.</exception>
    /// <exception cref="ArgumentNullException">There is no compatible System.Text.Json.Serialization.JsonConverter for T or its serializable members.</exception>
    /// <exception cref="NotSupportedException">utf8Json is null</exception>
    public static Task<T?> ReadAsObjectAsync<T>(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        return ReadAsObjectAsync<T>(httpContent, CaseInsensitiveOptions, cancellationToken);
    }

    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner.
    /// </summary>
    /// <param name="httpContent">The HTTP content to read.</param>
    /// <param name="options">The serializer options to use when deserializing the JSON.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type of the POCO to deserialize the JSON into.</typeparam>
    /// <returns>Returns an instance of <typeparamref name="T"/> or null if the stream is empty.</returns>
    /// <exception cref="JsonException">The JSON is invalid. -or- T is not compatible with the JSON. -or- There is remaining data in the stream.</exception>
    /// <exception cref="ArgumentNullException">There is no compatible System.Text.Json.Serialization.JsonConverter for T or its serializable members.</exception>
    /// <exception cref="NotSupportedException">utf8Json is null</exception>
    public static async Task<T?> ReadAsObjectAsync<T>(
        this HttpContent httpContent,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await httpContent.ReadAsStreamAsync(cancellationToken);

        if (stream.Length <= 0)
        {
            return default;
        }

        var result = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);

        return result;
    }
}
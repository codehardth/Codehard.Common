using System.Text.Json;

namespace Codehard.Common.Extensions;

public static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner,
    /// with default serializer options that ignores property name casing.
    /// </summary>
    /// <param name="httpContent"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns an instance of <see cref="T"/></returns>
    /// <exception cref="JsonException">The JSON is invalid. -or- T is not compatible with the JSON. -or- There is remaining data in the stream.</exception>
    /// <exception cref="ArgumentNullException">There is no compatible System.Text.Json.Serialization.JsonConverter for T or its serializable members.</exception>
    /// <exception cref="NotSupportedException">utf8Json is null</exception>
    public static Task<T?> ReadAsObjectAsync<T>(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        return ReadAsObjectAsync<T>(httpContent, DefaultOptions, cancellationToken);
    }

    /// <summary>
    /// Read the HTTP content in JSON format as a POCO in an asynchronous manner.
    /// </summary>
    /// <param name="httpContent"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns an instance of <see cref="T"/></returns>
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
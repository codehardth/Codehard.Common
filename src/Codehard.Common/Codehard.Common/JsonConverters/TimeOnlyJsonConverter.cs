using System.Text.Json;
using System.Text.Json.Serialization;

namespace Codehard.Common.JsonConverters;

/// <summary>
/// Converts instances of <see cref="TimeOnly"/> to and from JSON.
/// </summary>
public sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    /// <inheritdoc/>
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Converts a string representation of a time to a TimeOnly instance.
        return TimeOnly.Parse(reader.GetString()!);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        // Formats a TimeOnly instance as an ISO-8601 time string and writes it to the JSON writer.
        var isoTime = value.ToString("O");
        writer.WriteStringValue(isoTime);
    }
}
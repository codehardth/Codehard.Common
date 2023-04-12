using System.Text.Json;
using System.Text.Json.Serialization;

namespace Codehard.Common.JsonConverters;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that can be used to serialize and deserialize <see cref="DateOnly"/> instances.
/// </summary>
public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /// <inheritdoc />
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the string representation of a date in ISO 8601 format (yyyy-MM-dd) into a DateOnly instance.
        return DateOnly.Parse(reader.GetString()!);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        // Write the date as an ISO 8601 formatted string.
        var isoDate = value.ToString("O");
        writer.WriteStringValue(isoDate);
    }
}
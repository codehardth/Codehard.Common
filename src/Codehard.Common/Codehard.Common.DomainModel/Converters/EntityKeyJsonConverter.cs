using System.Text.Json;
using System.Text.Json.Serialization;

namespace Codehard.Common.DomainModel.Converters;

public class IntegerKeyJsonConverter : JsonConverter<IntegerKey>
{
    public override IntegerKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new IntegerKey(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, IntegerKey value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

public class LongKeyJsonConverter : JsonConverter<LongKey>
{
    public override LongKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new LongKey(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, LongKey value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

public class StringKeyJsonConverter : JsonConverter<StringKey>
{
    public override StringKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new StringKey(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, StringKey value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}

public class GuidKeyJsonConverter : JsonConverter<GuidKey>
{
    public override GuidKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new GuidKey(reader.GetGuid());
    }

    public override void Write(Utf8JsonWriter writer, GuidKey value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
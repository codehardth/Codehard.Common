using System.Text.Json;
using System.Text.Json.Serialization;

namespace Codehard.Common.DomainModel.Converters;

public class EntityKeyJsonConverter : JsonConverter<IEntityKey>
{
    public override IEntityKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IEntityKey value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
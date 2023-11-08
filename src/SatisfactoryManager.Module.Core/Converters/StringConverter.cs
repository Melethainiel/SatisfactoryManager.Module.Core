using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactoryManager.Module.Core.Converters;

public class StringNullableConverter : JsonConverter<string?>
{
    public override string? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? null : value;
    }

    public override void Write(
        Utf8JsonWriter writer,
        string? value,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
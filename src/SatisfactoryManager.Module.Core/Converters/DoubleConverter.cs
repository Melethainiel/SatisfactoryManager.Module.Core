using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactoryManager.Module.Core.Converters;

public class DoubleNullableConverter : JsonConverter<double?>
{
    public override double? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null) return null;

        if (double.TryParse(
                value,
                NumberStyles.Number,
                new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                },
                out var doubleValue))
            return doubleValue;
        return null;
    }

    public override void Write(
        Utf8JsonWriter writer,
        double? value,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

public class DoubleConverter : JsonConverter<double>
{
    public override double Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is null) return 0;

        if (double.TryParse(
                value,
                NumberStyles.Number,
                new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                },
                out var doubleValue))
            return doubleValue;
        return 0;
    }

    public override void Write(
        Utf8JsonWriter writer,
        double value,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
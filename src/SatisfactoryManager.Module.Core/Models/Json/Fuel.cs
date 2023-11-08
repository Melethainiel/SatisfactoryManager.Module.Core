using System.Text.Json.Serialization;
using SatisfactoryManager.Module.Core.Converters;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class Fuel
{
    [JsonConverter(typeof(StringNullableConverter))]
    public string? MSupplementalResourceClass { get; set; }

    public string MFuelClass { get; set; }

    [JsonConverter(typeof(DoubleNullableConverter))]

    public double? MByproductAmount { get; set; }

    [JsonConverter(typeof(StringNullableConverter))]
    public string? MByproduct { get; set; }
}
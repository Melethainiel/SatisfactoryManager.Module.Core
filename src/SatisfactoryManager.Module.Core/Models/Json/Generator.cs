using System.Text.Json.Serialization;
using SatisfactoryManager.Module.Core.Converters;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class Generator
{
    public string ClassName { get; set; } = null!;
    public string MDisplayName { get; set; } = null!;

    [JsonConverter(typeof(DoubleNullableConverter))]
    public double? MPowerConsumption { get; set; }

    [JsonConverter(typeof(DoubleConverter))]
    public double MPowerProduction { get; set; }

    [JsonConverter(typeof(DoubleNullableConverter))]
    public double? MSupplementalLoadAmount { get; set; }

    public string MDescription { get; set; } = null!;
    public Fuel[]? MFuel { get; set; }
}
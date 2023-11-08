using System.Text.Json.Serialization;
using SatisfactoryManager.Module.Core.Converters;

namespace SatisfactoryManager.Module.Core.Models.Json;

public class Item
{
    public string ClassName { get; set; } = null!;
    public string MDisplayName { get; set; } = null!;
    public string MDescription { get; set; } = null!;
    public string MPersistentBigIcon { get; set; } = null!;
    public string MForm { get; set; } = null!;

    [JsonConverter(typeof(DoubleNullableConverter))]
    public double? MEnergyValue { get; set; }

    [JsonIgnore]
    public string GroupName { get; set; }

    [JsonIgnore]
    public double? EnergyValue => MEnergyValue / (MForm == "RF_SOLID" ? 1000 : 1);
}
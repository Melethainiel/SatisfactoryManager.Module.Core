using System.Globalization;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class Manufacturer
{
    private double? _powerConsumption;
    public string ClassName { get; set; } = null!;
    public string MDisplayName { get; set; } = null!;
    public string MPowerConsumption { get; set; } = null!;
    public string MManufacturingSpeed { get; set; } = null!;
    public string MDescription { get; set; } = null!;
    public string? MEstimatedMaximumPowerConsumption { get; set; }

    public double PowerConsumption
    {
        get
        {
            return _powerConsumption ??= double.Parse(
                !string.IsNullOrEmpty(MEstimatedMaximumPowerConsumption)
                    ? MEstimatedMaximumPowerConsumption
                    : MPowerConsumption,
                NumberStyles.Number,
                new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                });
        }
    }
}
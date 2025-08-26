using System.Globalization;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class Miner
{
    private string[]? _allowedResourceForms;
    private string[]? _allowedResources;
    private int? _itemsPerMinute;
    private double? _powerConsumption;

    public string ClassName { get; set; } = null!;
    public string? MItemsPerCycle { get; set; }
    public string MAllowedResourceForms { get; set; } = null!;
    public string? MExtractCycleTime { get; set; }
    public string MAllowedResources { get; set; } = null!;
    public string MPowerConsumption { get; set; } = null!;
    public string MDisplayName { get; set; } = null!;
    public string MDescription { get; set; } = null!;
    public string? MParticleMap { get; set; }


    public string[] AllowedResourceForms =>
        _allowedResourceForms ??= string.IsNullOrEmpty(MAllowedResourceForms)
            ? Array.Empty<string>()
            : MAllowedResourceForms[1..^1]
                .Split(',');

    public string[] AllowedResources =>
        _allowedResources ??= GetAllowedResourcesFromData();

    private string[] GetAllowedResourcesFromData()
    {
        // Try MAllowedResources first
        if (!string.IsNullOrEmpty(MAllowedResources))
        {
            return MAllowedResources[1..^1]
                .Split(',')
                .Select(
                    x => x.Split(".")
                        .Last()
                        .Replace("\"", "")
                        .Replace("\'", ""))
                .ToArray();
        }
        
        // Fallback to mParticleMap if MAllowedResources is empty
        if (!string.IsNullOrEmpty(MParticleMap))
        {
            return ExtractResourcesFromParticleMap(MParticleMap);
        }
        
        return Array.Empty<string>();
    }
    
    private static string[] ExtractResourcesFromParticleMap(string particleMap)
    {
        // Extract resources from particle map format:
        // ((ResourceNode_...="/Game/.../Desc_Coal.Desc_Coal_C",...)
        var resources = new List<string>();
        
        // Find all resource paths in the particle map
        var matches = System.Text.RegularExpressions.Regex.Matches(
            particleMap, 
            @"/Game/[^""]*/(Desc_\w+\.Desc_\w+_C)");
        
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                // Extract the final part after the last dot (e.g., "Desc_OreIron.Desc_OreIron_C" -> "Desc_OreIron_C")
                var fullMatch = match.Groups[1].Value;
                var finalPart = fullMatch.Split('.').Last();
                resources.Add(finalPart);
            }
        }
        
        return resources.Distinct().ToArray();
    }

    public double PowerConsumption =>
        _powerConsumption ??= double.Parse(
            MPowerConsumption,
            NumberStyles.Number,
            new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            });

    public int ItemsPerMinute
    {
        get
        {
            if (_itemsPerMinute is not null) return _itemsPerMinute.Value;
            var multiplicator = MAllowedResourceForms switch
            {
                "(RF_SOLID)" => 60,
                "(RF_LIQUID,RF_GAS)" => new decimal(0.06),
                "(RF_LIQUID)" => new decimal(0.06),
                "(RF_LIQUID,RF_GAS,RF_HEAT)" => new decimal(0.06),
                _ => throw new Exception($"Unknown resource form {MAllowedResourceForms}")
            };

            var itemsPerCycle = MItemsPerCycle is null ? 0 : int.Parse(MItemsPerCycle);
            var extractCycleTime = MExtractCycleTime is null
                ? 1
                : decimal.Parse(
                    MExtractCycleTime,
                    NumberStyles.Number,
                    new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    });
            var itemsPerMinute = itemsPerCycle / extractCycleTime * multiplicator;
            _itemsPerMinute = decimal.ToInt32(itemsPerMinute);
            return _itemsPerMinute.Value;
        }
    }
}
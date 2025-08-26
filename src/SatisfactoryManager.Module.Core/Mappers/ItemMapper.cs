using SatisfactoryManager.Module.Core.Models.Dto;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Mappers;

public static class ItemMapper
{
    public static Dictionary<string, string[]> CreateItemToBuildingMapping(IEnumerable<Miner> miners)
    {
        var mapping = new Dictionary<string, string[]>();
        
        foreach (var miner in miners)
        {
            foreach (var allowedResource in miner.AllowedResources)
            {
                if (!mapping.ContainsKey(allowedResource))
                {
                    mapping[allowedResource] = new[] { miner.ClassName };
                }
                else
                {
                    mapping[allowedResource] = mapping[allowedResource]
                        .Concat(new[] { miner.ClassName })
                        .Distinct()
                        .ToArray();
                }
            }
        }
        
        return mapping;
    }

    public static Dictionary<string, string[]> CreateItemToGeneratorMapping(IEnumerable<Generator> generators)
    {
        var mapping = new Dictionary<string, string[]>();
        
        foreach (var generator in generators)
        {
            if (generator.MFuel == null) continue;
            
            foreach (var fuel in generator.MFuel)
            {
                // Extract the final part after the last dot/slash (e.g., "Desc_Coal_C" from full path)
                var fuelClassName = fuel.MFuelClass.Split('.', '/').Last().Replace("\"", "").Replace("'", "");
                
                if (!mapping.ContainsKey(fuelClassName))
                {
                    mapping[fuelClassName] = new[] { generator.ClassName };
                }
                else
                {
                    mapping[fuelClassName] = mapping[fuelClassName]
                        .Concat(new[] { generator.ClassName })
                        .Distinct()
                        .ToArray();
                }
            }
        }
        
        return mapping;
    }

    public static ItemDto ToDto(this Item item, string[] extractionBuildings, string[] fuelGenerators)
    {
        return new ItemDto(
            item.ClassName,
            item.MDisplayName,
            item.MDescription.Replace("\r\n", ""),
            item.MForm,
            item.EnergyValue,
            extractionBuildings,
            fuelGenerators);
    }
}
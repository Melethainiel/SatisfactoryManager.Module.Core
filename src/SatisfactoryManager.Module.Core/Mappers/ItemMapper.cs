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
                    mapping[allowedResource] = new[] { miner.MDisplayName };
                }
                else
                {
                    mapping[allowedResource] = mapping[allowedResource]
                        .Concat(new[] { miner.MDisplayName })
                        .Distinct()
                        .ToArray();
                }
            }
        }
        
        return mapping;
    }

    public static ItemDto ToDto(this Item item, string[] extractionBuildings)
    {
        return new ItemDto(
            item.ClassName,
            item.MDisplayName,
            item.MDescription.Replace("\r\n", ""),
            item.MForm,
            item.EnergyValue,
            extractionBuildings);
    }
}
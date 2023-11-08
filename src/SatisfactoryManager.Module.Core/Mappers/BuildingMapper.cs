using SatisfactoryManager.Module.Core.Models.Dto;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Mappers;

public static class BuildingMapper
{
    public static BuildingDto ToDto(this Generator generator)
    {
        return new BuildingDto(
            generator.ClassName,
            generator.MDisplayName,
            null,
            generator.MPowerConsumption,
            generator.MPowerProduction,
            generator.MSupplementalLoadAmount,
            BuildingTypeDto.Generator,
            generator.MFuel?.Select(f => f.ToDto())
                .ToArray());
    }

    public static BuildingDto ToDto(this Miner miner)
    {
        return new BuildingDto(
            miner.ClassName,
            miner.MDisplayName,
            miner.ItemsPerMinute,
            miner.PowerConsumption,
            null,
            null,
            BuildingTypeDto.Miner,
            null);
    }

    public static BuildingDto ToDto(this Manufacturer manufacturer)
    {
        return new BuildingDto(
            manufacturer.ClassName,
            manufacturer.MDisplayName,
            null,
            manufacturer.PowerConsumption,
            null,
            null,
            BuildingTypeDto.Constructor,
            null);
    }

    public static IEnumerable<BuildingDto> ToDto(this IEnumerable<Generator> generators)
    {
        return generators.Select(ToDto);
    }

    public static IEnumerable<BuildingDto> ToDto(this IEnumerable<Miner> miners)
    {
        return miners.Select(ToDto);
    }

    public static IEnumerable<BuildingDto> ToDto(this IEnumerable<Manufacturer> manufacturers)
    {
        return manufacturers.Select(ToDto);
    }
}
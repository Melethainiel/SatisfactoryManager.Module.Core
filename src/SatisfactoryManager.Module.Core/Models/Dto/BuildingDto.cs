using YamlDotNet.Serialization;

namespace SatisfactoryManager.Module.Core.Models.Dto;

public record BuildingDto(
    string ClassName,
    string Name,
    int? Output,
    double? EnergyConsumption,
    double? EnergyProduction,
    double? SupplementalLoadAmount,
    BuildingTypeDto Type,
    int? ProductionShardSlotSize,
    [property: YamlIgnore] FuelDto[]? Fuels);
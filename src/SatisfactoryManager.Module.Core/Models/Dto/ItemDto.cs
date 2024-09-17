namespace SatisfactoryManager.Module.Core.Models.Dto;

public record ItemDto(
    string ClassName,
    string DisplayName,
    string? Description,
    string Form,
    double? EnergyValue);
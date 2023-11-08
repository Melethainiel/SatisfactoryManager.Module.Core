namespace SatisfactoryManager.Module.Core.Models.Dto;

public record FuelDto(
    string Item,
    string? SupplementalResource,
    string? ByProduct,
    double? ByProductAmount);
namespace SatisfactoryManager.Module.Core.Models.Dto;

public record ItemDto(
    string ClassName,
    string Name,
    double? EnergyValue,
    RecipeDto? Recipe,
    string? AlternativeOf);
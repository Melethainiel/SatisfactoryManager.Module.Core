using YamlDotNet.Serialization;

namespace SatisfactoryManager.Module.Core.Models.Dto;

public record ItemDto(
    [property: YamlIgnore] string ClassName,
    string Name,
    double? EnergyValue,
    RecipeDto? Recipe,
    string? AlternativeOf);
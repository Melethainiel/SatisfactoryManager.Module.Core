namespace SatisfactoryManager.Module.Core.Models.Dto;

public record RecipeDto(
    string ClassName,
    string DisplayName,
    int ManufacturingDuration,
    RecipePartDto[]? Ingredients,
    RecipePartDto[]? Products,
    string[]? CraftedIn);
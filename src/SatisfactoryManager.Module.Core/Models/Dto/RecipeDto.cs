namespace SatisfactoryManager.Module.Core.Models.Dto;

public record RecipeDto(
    string ClassName,
    string DisplayName,
    int ManufactoringDuration,
    RecipePartDto[]? Ingredients,
    RecipePartDto[]? Products,
    string[]? CraftedIn);
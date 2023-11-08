namespace SatisfactoryManager.Module.Core.Models.Dto;

public record RecipeDto(
    int Time,
    RecipePartDto[]? Inputs,
    RecipePartDto[]? Outputs,
    string[]? CraftedIn);
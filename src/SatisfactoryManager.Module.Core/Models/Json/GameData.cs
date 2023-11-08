namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class GameData
{
    public required Dictionary<string, Item> GameItems { get; init; }

    public required Dictionary<string, Item> GameResources { get; init; }

    public required IReadOnlyCollection<Recipe> GameRecipes { get; init; }
    public required IReadOnlyCollection<Miner> GameMiners { get; init; }

    public required IReadOnlyCollection<Manufacturer> GameManufacturers { get; init; }

    public required IReadOnlyCollection<Generator> GameGenerators { get; init; }
}
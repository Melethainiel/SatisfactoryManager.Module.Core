using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class GameDataBuilder
{
    private static GameDataBuilder? _instance;

    private static readonly string[] ManufacturerClassNames =
    {
        "Class'/Script/FactoryGame.FGBuildableManufacturer'",
        "Class'/Script/FactoryGame.FGBuildableManufacturerVariablePower'"
    };

    private static readonly string[] GeneratorClassNames =
    {
        "Class'/Script/FactoryGame.FGBuildableGeneratorFuel'",
        "Class'/Script/FactoryGame.FGBuildableGeneratorNuclear'"
    };

    private static readonly string[] MinerClassNames =
    {
        "Class'/Script/FactoryGame.FGBuildableResourceExtractor'",
        "Class'/Script/FactoryGame.FGBuildableWaterPump'",
        "Class'/Script/FactoryGame.FGBuildableFrackingExtractor'",
        "Class'/Script/FactoryGame.FGBuildableFrackingActivator'"
    };

    private static readonly string[] ItemClassNames =
    {
        "Class'/Script/FactoryGame.FGItemDescriptor'",
        "Class'/Script/FactoryGame.FGResourceDescriptor'",
        "Class'/Script/FactoryGame.FGItemDescriptorBiomass'",
        "Class'/Script/FactoryGame.FGItemDescAmmoTypeColorCartridge'",
        "Class'/Script/FactoryGame.FGItemDescriptorNuclearFuel'"
    };

    private static readonly string[] ResourcesClassNames =
    {
        "Class'/Script/FactoryGame.FGResourceDescriptor'"
    };

    private static readonly string[] RecipesClassNames =
    {
        "Class'/Script/FactoryGame.FGRecipe'"
    };


    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    private Dictionary<string, JsonRawData>? _baseData;
    private List<Generator>? _generators;
    private List<Item>? _items;
    private List<Manufacturer>? _manufacturers;
    private List<Miner>? _miners;
    private List<Recipe>? _recipes;
    private List<Item>? _resources;

    private GameDataBuilder()
    {
    }

    public static GameDataBuilder Default => _instance ??= new GameDataBuilder();

    public GameDataBuilder AddRawData(IEnumerable<JsonRawData> jsonRawData)
    {
        _baseData = jsonRawData.ToDictionary(i => i.NativeClass);
        return this;
    }

    public GameDataBuilder BuildManufacturers()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");

        _manufacturers = new List<Manufacturer>();
        foreach (var groupName in ManufacturerClassNames)
            _manufacturers.AddRange(
                _baseData[groupName]
                    .GetClasses<Manufacturer>(_options));

        return this;
    }

    public GameDataBuilder BuildGenerators()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");
        _generators = new List<Generator>();
        foreach (var groupName in GeneratorClassNames)
            _generators.AddRange(
                _baseData[groupName]
                    .GetClasses<Generator>(_options));

        return this;
    }

    public GameDataBuilder BuildMiners()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");
        _miners = new List<Miner>();
        foreach (var groupName in MinerClassNames)
            _miners.AddRange(
                _baseData[groupName]
                    .GetClasses<Miner>(_options));

        return this;
    }

    public GameDataBuilder BuildItems()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");
        _items = new List<Item>();
        foreach (var groupName in ItemClassNames)
        {
            var items = _baseData[groupName]
                .GetClasses<Item>(_options, i => !i.MPersistentBigIcon.Contains("/Events/"));
            items.ForEach(
                i => i.GroupName = groupName.Split('.')
                    .Last()
                    .Replace("\"", "")
                    .Replace("'", ""));
            _items.AddRange(items);
        }

        return this;
    }

    public GameDataBuilder BuildResources()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");
        _resources = new List<Item>();
        foreach (var groupName in ResourcesClassNames)
            _resources.AddRange(
                _baseData[groupName]
                    .GetClasses<Item>(_options, i => !i.MPersistentBigIcon.Contains("/Events/")));

        return this;
    }

    public GameDataBuilder BuildRecipes()
    {
        if (_baseData is null) throw new Exception($"Call {nameof(AddRawData)} first");
        _recipes = new List<Recipe>();
        foreach (var groupName in RecipesClassNames)
            _recipes.AddRange(
                _baseData[groupName]
                    .GetClasses<Recipe>(
                        _options,
                        i => string.IsNullOrEmpty(i.MRelevantEvents) && !i.MProduct.Contains("/Events/")));

        return this;
    }

    public GameData Build()
    {
        return new GameData
        {
            GameManufacturers = _manufacturers ?? new List<Manufacturer>(),
            GameGenerators = _generators ?? new List<Generator>(),
            GameMiners = _miners ?? new List<Miner>(),
            GameItems = _items?.ToDictionary(i => i.ClassName) ?? new Dictionary<string, Item>(),
            GameResources = _resources?.ToDictionary(i => i.ClassName) ?? new Dictionary<string, Item>(),
            GameRecipes = _recipes ?? new List<Recipe>()
        };
    }
}
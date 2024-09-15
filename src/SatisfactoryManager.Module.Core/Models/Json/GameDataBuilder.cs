using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class GameDataBuilder
{
    private static GameDataBuilder? _instance;

    private static readonly string[] ManufacturerClassNames =
    [
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableManufacturer'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableManufacturerVariablePower'"
    ];

    private static readonly string[] GeneratorClassNames =
    [
       "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableGeneratorFuel'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableGeneratorNuclear'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableGeneratorGeoThermal'"
    ];

    private static readonly string[] MinerClassNames =
    [
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableResourceExtractor'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableFrackingExtractor'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableWaterPump'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGBuildableFrackingActivator'",
    ];

    private static readonly string[] ItemClassNames =
    [
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGResourceDescriptor'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGItemDescriptor'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGItemDescriptorBiomass'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGAmmoTypeProjectile'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGAmmoTypeInstantHit'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGAmmoTypeSpreadshot'",
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGItemDescriptorNuclearFuel'",
    ];

    private static readonly string[] ResourcesClassNames =
    [
        "/Script/CoreUObject.Class'/Script/FactoryGame.FGResourceDescriptor'",
    ];

    private static readonly string[] RecipesClassNames =
    [
 "/Script/CoreUObject.Class'/Script/FactoryGame.FGRecipe'"   
  ];


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
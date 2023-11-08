using SatisfactoryManager.Module.Core.Mappers;
using GameDataParser.Models;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;
using SatisfactoryManager.Module.Core.Arguments;
using SatisfactoryManager.Module.Core.Models.Dto;
using SatisfactoryManager.Module.Core.Models.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SatisfactoryManager.Module.Core.Handlers;

public class YamlWriter(IOptions<ToolArguments> arguments) : IYamlWriter
{
    private readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .DisableAliases()
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
        .Build();

    public async Task<OneOf<Success, Error<string>>> RunAsync(
        GameData data,
        CancellationToken cancellationToken = default)
    {
        var directory = new DirectoryInfo(arguments.Value.OutputDirectoryPath);
        if (!directory.Exists) directory.Create();

        await WriteBuildingsAsync(
            data,
            directory,
            cancellationToken);

        await WriteItemsAsync(
            data,
            directory,
            cancellationToken);

        return new Success();
    }

    private async Task<OneOf<Success, Error<string>>> WriteBuildingsAsync(
        GameData data,
        DirectoryInfo directory,
        CancellationToken cancellationToken = default)
    {
        var buildings = new List<BuildingDto>();
        buildings.AddRange(data.GameGenerators.ToDto());
        buildings.AddRange(data.GameManufacturers.ToDto());
        buildings.AddRange(data.GameMiners.ToDto());

        var buildingsFile = new FileInfo(Path.Combine(directory.FullName, "buildings.yaml"));
        await File.WriteAllTextAsync(
            buildingsFile.FullName,
            _serializer.Serialize(buildings.ToDictionary(i => i.ClassName)),
            cancellationToken);

        return new Success();
    }

    private async Task<OneOf<Success, Error<string>>> WriteItemsAsync(
        GameData data,
        DirectoryInfo directory,
        CancellationToken cancellationToken = default)
    {
        var items = new List<ItemDto>();

        items.AddRange(GetItems(data));
        items.AddRange(GetResources(data));

        var itemsFile = new FileInfo(Path.Combine(directory.FullName, "items.yaml"));
        await File.WriteAllTextAsync(
            itemsFile.FullName,
            _serializer.Serialize(items.ToDictionary(i => i.ClassName)),
            cancellationToken);

        return new Success();
    }

    private static IEnumerable<ItemDto> GetResources(GameData data)
    {
        foreach (var gameResource in data.GameResources.Values)
        {
            var formBuildings = data.GameMiners.Where(i => i.AllowedResourceForms.Contains(gameResource.MForm))
                .Where(i => !i.AllowedResources.Any() || i.AllowedResources.Contains(gameResource.ClassName))
                .Select(i => i.ClassName)
                .ToArray();


            var recipe = new RecipeDto(
                1,
                null,
                null,
                formBuildings);


            var item = new ItemDto(
                gameResource.ClassName,
                gameResource.MDisplayName,
                gameResource.EnergyValue,
                recipe,
                null);

            yield return item;
        }
    }

    private static IEnumerable<ItemDto> GetItems(GameData data)
    {
        foreach (var recipe in data.GameRecipes)
        {
            if (!data.GameManufacturers.Any(i => recipe.ProducedIn.Contains(i.ClassName))) continue;

            var basicBuildingDtos = data.GameManufacturers
                .Where(i => recipe.ProducedIn.Any(j => j.Contains(i.ClassName)))
                .Select(i => i.ClassName)
                .ToArray();

            var alternateOf =
                recipe.Product.FirstOrDefault() is { } prod && data.GameItems.TryGetValue(prod.Name, out var alt)
                    ? alt.ClassName
                    : null;
            var isAlternate = recipe.MDisplayName.StartsWith("Alternate: ");
            
            var baseItem = data.GameItems.Values.SingleOrDefault(
                i => i.ClassName == recipe.ClassName || i.ClassName == alternateOf);

            if (baseItem is null)
            {
                Console.WriteLine($"Failed to find base item for {recipe.MDisplayName}");
                continue;
            }


            var inputs = GetRecipeParts(
                data,
                recipe,
                recipe.Ingredients);

            var outputs = GetRecipeParts(
                data,
                recipe,
                recipe.Product);

            var recipeDto = new RecipeDto(
                recipe.ManufactoringDuration,
                inputs.ToArray(),
                outputs.ToArray(),
                basicBuildingDtos);


            var item = new ItemDto(
                recipe.ClassName,
                recipe.MDisplayName.Replace("Alternate: ", ""),
                baseItem.EnergyValue,
                recipeDto,
                isAlternate ? alternateOf : null);

            yield return item;
        }
    }

    private static IEnumerable<RecipePartDto> GetRecipeParts(
        GameData data,
        Recipe recipe,
        IEnumerable<Ingredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            if (!data.GameItems.TryGetValue(ingredient.Name, out var itemDescription))
            {
                Console.WriteLine(ingredient.Name);
                continue;
            }

            if (!data.GameItems.TryGetValue(itemDescription.ClassName, out var itemPart))
            {
                Console.WriteLine(itemDescription.MDescription);
                continue;
            }

            var amountMultiplier = itemDescription.MForm == "RF_SOLID" ? 60 : 0.06;
            var part = new RecipePartDto(
                itemPart.ClassName,
                amountMultiplier * ingredient.Amount / recipe.ManufactoringDuration);
            yield return part;
        }
    }
}
using System.Text.Json;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Handlers;

public class JsonParser(IOptions<ToolArguments> arguments) : IJsonParser
{
    public async Task<OneOf<GameData, Error<string>>> ReadAsync(CancellationToken cancellationToken = default)
    {
        var fileInfo = new FileInfo(arguments.Value.InputFilePath);
        if (!fileInfo.Exists) return new Error<string>($"File \"{fileInfo.Name}\" not found");


        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var gameDataStr = await File.ReadAllTextAsync(fileInfo.FullName, cancellationToken);
        var gameData = JsonSerializer.Deserialize<JsonRawData[]>(gameDataStr, options);
        if (gameData is null) return new Error<string>($"Failed to read \"{fileInfo.Name}\"");

        GameDataBuilder.Default.AddRawData(gameData);


        GameDataBuilder.Default.BuildManufacturers()
            .BuildGenerators()
            .BuildMiners();

        GameDataBuilder.Default.BuildItems()
            .BuildResources()
            .BuildRecipes();

        var data = GameDataBuilder.Default.Build();

        return data;
    }
}
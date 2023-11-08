using SatisfactoryManager.Module.Core.Handlers;

namespace SatisfactoryManager.Module.Core;

public sealed class Orchestrator(
    IJsonParser jsonParser,
    IYamlWriter yamlWriter)
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var jsonResponse = await jsonParser.ReadAsync(cancellationToken);
        if (jsonResponse.IsT1)
        {
            Console.WriteLine(jsonResponse.AsT1);
            return;
        }

        var yamlResponse = await yamlWriter.RunAsync(jsonResponse.AsT0, cancellationToken);
    }
}
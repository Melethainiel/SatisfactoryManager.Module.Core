using OneOf;
using OneOf.Types;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Handlers;

public interface IYamlWriter
{
    Task<OneOf<Success, Error<string>>> RunAsync(
        GameData data,
        CancellationToken cancellationToken = default);
}
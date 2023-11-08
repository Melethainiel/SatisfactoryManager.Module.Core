using OneOf;
using OneOf.Types;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Handlers;

public interface IJsonParser
{
    Task<OneOf<GameData, Error<string>>> ReadAsync(CancellationToken cancellationToken = default);
}
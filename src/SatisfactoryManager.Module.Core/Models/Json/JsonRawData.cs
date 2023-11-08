using System.Text.Json;

namespace SatisfactoryManager.Module.Core.Models.Json;

public sealed class JsonRawData
{
    public string NativeClass { get; set; } = null!;
    public JsonElement[] Classes { get; set; } = null!;

    public List<T> GetClasses<T>(
        JsonSerializerOptions jsonSerializerOptions,
        Func<T, bool>? filter = null)
    {
        return Classes.Select(i => i.Deserialize<T>(jsonSerializerOptions)!)
                   .Where(filter ?? (_ => true))
                   .ToList()
               ?? throw new Exception("Could not deserialize classes");
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace SatisfactoryManager.Module.Core.Handlers;

public static class ServiceInstaller
{
    public static void AddHandler(this IServiceCollection services)
    {
        services.AddSingleton<IJsonParser, JsonParser>();
        services.AddSingleton<IYamlWriter, YamlWriter>();
    }
}
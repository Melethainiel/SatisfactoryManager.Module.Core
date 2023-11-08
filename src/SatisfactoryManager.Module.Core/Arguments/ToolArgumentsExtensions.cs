using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace SatisfactoryManager.Module.Core.Arguments;

public static class ToolArgumentsExtensions
{
    public static void AddToolArguments(this IServiceCollection services, string[] args)
    {
        
        services.AddOptions<ToolArguments>()
            .Configure(opt => Parser.Default.ParseArguments(() => opt, args)
                .WithNotParsed(_ => Environment.Exit(-1)));
    }
}
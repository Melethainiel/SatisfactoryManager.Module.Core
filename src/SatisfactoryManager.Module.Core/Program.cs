using CommandLine;
using SatisfactoryManager.Module.Core;
using SatisfactoryManager.Module.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello World!");

var services = new ServiceCollection();

services.AddOptions<ToolArguments>()
    .Configure(opt => Parser.Default.ParseArguments(() => opt, args));
services.AddSingleton<Orchestrator>();
services.AddHandler();


var serviceProvider = services.BuildServiceProvider();

var orchestrator = serviceProvider.GetRequiredService<Orchestrator>();

await orchestrator.RunAsync(CancellationToken.None);
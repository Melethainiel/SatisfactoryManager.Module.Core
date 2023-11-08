using System.Security.Cryptography;
using CommandLine;
using SatisfactoryManager.Module.Core;
using SatisfactoryManager.Module.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;
using SatisfactoryManager.Module.Core.Arguments;

Console.WriteLine("Hello World!");

var services = new ServiceCollection();

services.AddToolArguments(args);
services.AddSingleton<Orchestrator>();
services.AddHandler();


var serviceProvider = services.BuildServiceProvider();

var orchestrator = serviceProvider.GetRequiredService<Orchestrator>();
var token = new CancellationTokenSource();

Console.CancelKeyPress += (
    _,
    _) => token.Cancel();

try
{
    await orchestrator.RunAsync(token.Token);
}
catch (TaskCanceledException e)
{
    Console.WriteLine("Task canceled. Shutting down app");
}

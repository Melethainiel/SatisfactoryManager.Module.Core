using CommandLine;

namespace SatisfactoryManager.Module.Core;

public sealed class ToolArguments
{
    [Option(
        'i',
        "input",
        HelpText = "Path to the CoreData file to parse.")]
    public string InputFilePath { get; set; } = "Data/Docs.json";

    [Option(
        'o',
        "output",
        HelpText = "Path to the output directory.")]
    public string OutputDirectoryPath { get; set; } = "Data/Output";
}
using CommandLine;
using JetBrains.Annotations;
using NoMercy.Networking;

namespace NoMercy.Server;


[UsedImplicitly]
public class StartupArgs
{
    // dev
    [Option('d', "dev", Required = false, HelpText = "Run the server in development mode.")]
    public bool Dev { get; [UsedImplicitly] set; }

    [Option("logLevel", Required = false, HelpText = "Defines which minimum log level to keep in logs.")]
    public string LogLevel { get; [UsedImplicitly] set; } = "info";
    
    [Option("seed", Required = false, HelpText = "Run the server in development mode.")]
    public bool Seed { get; [UsedImplicitly] set; }
    
    [Option('i', "internal-port", Required = false, HelpText = "Internal port to use for the server.")]
    public int InternalPort { get; [UsedImplicitly] set; } = NoMercyConfig.InternalServerPort;

    [Option('e', "external-port", Required = false, HelpText = "External port to use for the server.")]
    public int ExternalPort { get;[UsedImplicitly] set; } = NoMercyConfig.ExternalServerPort;

}
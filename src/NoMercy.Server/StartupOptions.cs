using CommandLine;

namespace NoMercy.Server;

public class StartupOptions
{
    // dev
    [Option('d', "dev", Required = false, HelpText = "Run the server in development mode.")]
    public bool Dev { get; set; }
    
    [Option("logLevel", Required = false, HelpText = "Run the server in development mode.")]
    public string LogLevel { get; set; }
    
    [Option("seed", Required = false, HelpText = "Run the server in development mode.")]
    public bool Seed { get; set; }
    
    [Option('i', "internal-port", Required = false, HelpText = "Internal port to use for the server.")]
    public int InternalPort { get; set; }
    
    [Option('e', "external-port", Required = false, HelpText = "External port to use for the server.")]
    public int ExternalPort { get; set; }

}
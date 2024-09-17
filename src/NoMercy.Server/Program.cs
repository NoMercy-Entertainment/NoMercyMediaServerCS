using System.Diagnostics;
using System.Net;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CommandLine;
using Microsoft.AspNetCore;
using NoMercy.Data.Logic;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Providers.AniDb.Clients;
using NoMercy.Queue;
using NoMercy.Server.App.Helper;
using NoMercy.Server.StartupServices.TrayIcon;
using Serilog.Events;
using AppFiles = NoMercy.NmSystem.AppFiles;

namespace NoMercy.Server;
public static class Program
{
    private static bool ShouldSeedMarvel { get; set; }
    
    public static Task Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
        {
            var exception = (Exception)eventArgs.ExceptionObject;
            Logger.App("UnhandledException " + exception);
        };

        Console.CancelKeyPress += (_, _) =>
        {
            Shutdown().Wait();
            Environment.Exit(0);
        };

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Logger.App("SIGTERM received, shutting down.");
            Shutdown().Wait();
        };

        return Parser.Default.ParseArguments<StartupArgs>(args)
            .MapResult(Start, ErrorParsingArguments);

        static Task ErrorParsingArguments(IEnumerable<Error> errors)
        {
            Environment.ExitCode = 1;
            return Task.CompletedTask;
        }
    }

    private async static Task Start(StartupArgs options)
    {
        Console.Clear();
        Console.Title = "NoMercy Server";
        
        if (options.Dev)
        {
            Logger.App("Running in development mode.");
            
            NoMercyConfig.IsDev = true;
            
            NoMercyConfig.AppBaseUrl = "https://app-dev.nomercy.tv/";
            NoMercyConfig.ApiBaseUrl = "https://api-dev.nomercy.tv/";
            NoMercyConfig.ApiServerBaseUrl = $"{NoMercyConfig.ApiBaseUrl}v1/server/";
            
            NoMercyConfig.AuthBaseUrl = "https://auth-dev.nomercy.tv/realms/NoMercyTV/";
            NoMercyConfig.TokenClientSecret = "1lHWBazSTHfBpuIzjAI6xnNjmwUnryai";
        }
        
        if(options.Seed)
        {
            Logger.App("Seeding database.");
            ShouldSeedMarvel = true;
        }
        
        if (!string.IsNullOrEmpty(options.LogLevel))
        {
            Logger.App($"Setting log level to: {options.LogLevel}.");
            Logger.SetLogLevel(Enum.Parse<LogEventLevel>(options.LogLevel.ToTitleCase()));
        }
        
        Logger.App(NoMercyConfig.AuthBaseUrl);

        if (options.InternalPort != 0)
        {
            Logger.App("Setting internal port to " + options.InternalPort);
            NoMercyConfig.InternalServerPort = options.InternalPort;
        }
        
        if (options.ExternalPort != 0)
        {
            Logger.App("Setting external port to " + options.ExternalPort);
            NoMercyConfig.ExternalServerPort = options.ExternalPort;
        }

        Stopwatch stopWatch = new();
        stopWatch.Start();

        Databases.QueueContext = new QueueContext();
        Databases.MediaContext = new MediaContext();

        await Init();

        IWebHost app = CreateWebHostBuilder(new WebHostBuilder()).Build();

        app.Services.GetService<IHostApplicationLifetime>()?.ApplicationStarted.Register(() =>
        {
            Task.Run(() =>
            {
                stopWatch.Stop();

                Task.Delay(300).Wait();

                Logger.App($"Internal Address: {Networking.Networking.InternalAddress}");
                Logger.App($"External Address: {Networking.Networking.ExternalAddress}");

                ConsoleMessages.ServerRunning();

                Logger.App($"Server started in {stopWatch.ElapsedMilliseconds}ms");
            });
        });

        new Thread(() => app.RunAsync()).Start();
        new Thread(Dev.Run).Start();

        await Task.Delay(-1);
    }

    private async static Task Shutdown()
    {
        await Task.CompletedTask;
    }

    private async static Task Restart()
    {
        await Task.CompletedTask;
    }

    private static IWebHostBuilder CreateWebHostBuilder(this IWebHostBuilder _)
    {
        UriBuilder url = new ()
        {
            Host = IPAddress.Any.ToString(),
            Port = NoMercyConfig.InternalServerPort,
            Scheme = "https"
        };

        return WebHost.CreateDefaultBuilder([])
            .ConfigureKestrel(Certificate.KestrelConfig)
            .UseUrls(url.ToString())
            .UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.Limits.MaxRequestBodySize = null;
                options.Limits.MaxRequestBufferSize = null;
                options.Limits.MaxConcurrentConnections = null;
                options.Limits.MaxConcurrentUpgradedConnections = null;
            })
            .UseQuic()
            .UseSockets()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();
                services.AddSingleton<ISunsetPolicyManager, DefaultSunsetPolicyManager>();
            })
            .UseStartup<Startup>();
    }

    private async static Task Init()
    {
        ApiInfo.RequestInfo().Wait();

        List<TaskDelegate> startupTasks =
        [
            new (ConsoleMessages.Logo),
            new (AppFiles.CreateAppFolders),
            new (Networking.Networking.Discover),
            new (Auth.Init),
            new (() => Seed.Init(ShouldSeedMarvel)),
            new (Register.Init),
            new (Binaries.DownloadAll),
            // new (AniDbBaseClient.Init),
            new (TrayIconFactory.MakeIcon)
        ];

        AppDomain.CurrentDomain.ProcessExit += (_, _) => { AniDbBaseClient.Dispose(); };

        await RunStartup(startupTasks);
        
        await using MediaContext mediaContext = new();
        List<Configuration> configuration = mediaContext.Configuration.ToList();
        
        foreach (Configuration? config in configuration) {
            Logger.App($"Configuration: {config.Key} = {config.Value}");
            switch (config.Key) {
                case "InternalServerPort": NoMercyConfig.InternalServerPort = int.Parse(config.Value);
                    break;
                case "ExternalServerPort": NoMercyConfig.ExternalServerPort = int.Parse(config.Value);
                    break;
            }
        }

        Thread t = new(new Task(() => QueueRunner.Initialize().Wait()).Start)
        {
            Name = "Queue workers",
            Priority = ThreadPriority.Lowest,
            IsBackground = true
        };
        t.Start();
    }

    private async static Task RunStartup(List<TaskDelegate> startupTasks)
    {
        foreach (TaskDelegate task in startupTasks)
        {
            await task.Invoke();
        }
    }
}
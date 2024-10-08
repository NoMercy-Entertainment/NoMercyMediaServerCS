using System.Diagnostics;
using System.Net;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CommandLine;
using Microsoft.AspNetCore;
using NoMercy.Data.Logic;
using NoMercy.Database;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Queue;
using NoMercy.Server.app.Helper;
using AppFiles = NoMercy.NmSystem.AppFiles;

namespace NoMercy.Server;
public static class Program
{
    private static bool ShouldSeedMarvel { get; set; }
    
    public static Task Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
        {
            Exception exception = (Exception)eventArgs.ExceptionObject;
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

        return Parser.Default.ParseArguments<StartupOptions>(args)
            .MapResult(Start, ErrorParsingArguments);

        static Task ErrorParsingArguments(IEnumerable<Error> errors)
        {
            Environment.ExitCode = 1;
            return Task.CompletedTask;
        }
    }

    private static async Task Start(StartupOptions options)
    {
        Console.Clear();
        Console.Title = "NoMercy Server";

        options.ApplySettings(out bool shouldSeedMarvel);
        ShouldSeedMarvel = shouldSeedMarvel;

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

    private static async Task Shutdown()
    {
        await Task.CompletedTask;
    }

    private static async Task Restart()
    {
        await Task.CompletedTask;
    }

    private static IWebHostBuilder CreateWebHostBuilder(this IWebHostBuilder _)
    {
        UriBuilder url = new()
        {
            Host = IPAddress.Any.ToString(),
            Port = Config.InternalServerPort,
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

    private static async Task Init()
    {
        ApiInfo.RequestInfo().Wait();

        if (UserSettings.TryGetUserSettings(out var settings))
        {
            UserSettings.ApplySettings(settings);
        }

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
            new (TrayIcon.Make)
        ];

        // AppDomain.CurrentDomain.ProcessExit += (_, _) => { AniDbBaseClient.Dispose(); };

        await RunStartup(startupTasks);

        Thread t = new(new Task(() => QueueRunner.Initialize().Wait()).Start)
        {
            Name = "Queue workers",
            Priority = ThreadPriority.Lowest,
            IsBackground = true
        };
        t.Start();
    }

    private static async Task RunStartup(List<TaskDelegate> startupTasks)
    {
        foreach (TaskDelegate task in startupTasks)
        {
            await task.Invoke();
        }
    }
}
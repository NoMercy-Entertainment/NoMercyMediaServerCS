using NoMercy.Helpers.Monitoring;
using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.Api.Controllers.Socket;
public static class ResourceMonitor
{
    private static bool _broadcasting;
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static void StartBroadcasting()
    {
        Logger.Socket("Starting resource monitoring broadcast");
        _broadcasting = true;

        Task.Run(() => BroadcastLoop(CancellationTokenSource.Token));
    }

    public static void StopBroadcasting()
    {
        Logger.Socket("Stopping resource monitoring broadcast");
        _broadcasting = false;

        CancellationTokenSource.Cancel();
    }

    private static void BroadcastLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            DateTime time = DateTime.Now;
            if (_broadcasting && !cancellationToken.IsCancellationRequested) break;
            try
            {
                Resource? resourceData = Helpers.Monitoring.ResourceMonitor.Monitor();
                Networking.Networking.SendToAll("ResourceUpdate", "dashboardHub", resourceData);

                // at least one second between broadcasts
                TimeSpan timeDiff = DateTime.Now - time;
                if (timeDiff.TotalSeconds < 1) Thread.Sleep(1000 - (int)timeDiff.TotalMilliseconds);
            }
            catch (Exception e)
            {
                Logger.Socket($"Error broadcasting resource data: {e.Message}", LogEventLevel.Error);
            }
        }
    }
}
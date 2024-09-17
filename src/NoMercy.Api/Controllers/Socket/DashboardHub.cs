using Microsoft.AspNetCore.Http;
using NoMercy.Networking;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.Socket;

public class DashboardHub(IHttpContextAccessor httpContextAccessor) : ConnectionHub(httpContextAccessor) {

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        Logger.Socket("Dashboard client disconnected");
        StopResources();
    }

    public void StartResources()
    {
        ResourceMonitor.StartBroadcasting();
    }

    public void StopResources()
    {
        ResourceMonitor.StopBroadcasting();
    }
}
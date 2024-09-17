using H.NotifyIcon.Core;
using System.Drawing;
using System.Runtime.Versioning;

namespace NoMercy.Server.StartupServices.TrayIcon;

[SupportedOSPlatform("windows10.0.18362")]
public class WindowsTrayIcon(string iconPath)
{
    private Icon IconImage { get; } = new(iconPath);

    private TrayIconWithContextMenu? _trayIcon;
    private TrayIconWithContextMenu Icon => _trayIcon ??= new TrayIconWithContextMenu {
        Icon = IconImage.Handle,
        ToolTip = "NoMercy MediaServer C#",
        ContextMenu = new PopupMenu
        {
            Items =
            {
                // new PopupMenuItem("Show App", (_, _) => Show()),
                // new PopupMenuSeparator(),
                // new PopupMenuItem("Pause Server", (_, _) => Pause()),
                // new PopupMenuItem("Restart Server", (_, _) => Restart()),
                new PopupMenuItem("Shutdown", (_, _) => Shutdown())
            }
        }
    };

    public void Create() => Icon.Create();

    private void Pause()
    {
    }

    private void Show()
    {
    }

    private void Restart()
    {
    }

    private void Shutdown()
    {
        Icon.Dispose();
        Environment.Exit(0);
    }
}
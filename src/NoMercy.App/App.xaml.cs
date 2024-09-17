using System.Drawing;
using H.NotifyIcon.Core;

namespace NoMercy.App;

public partial class App : Application
{
    private readonly TrayIcon? _trayIcon;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        string iconPath = Path.Combine(AppContext.BaseDirectory, "appicon.ico");
        if (!File.Exists(iconPath)) throw new FileNotFoundException("Tray icon file not found", iconPath);

#pragma warning disable CA1416
        _trayIcon = new TrayIconWithContextMenu
        {
            ToolTip = "NoMercy MediaServer C#",
            Visibility = IconVisibility.Visible,
            IsDesignMode = false,
            Icon = new Icon(iconPath).Handle,
            WindowHandle = 0,
            UseStandardTooltip = false,
            ContextMenu = new PopupMenu
            {
                Items =
                {
                    // new PopupMenuItem("Open NoMercy.Maui",OpenApp),
                    new PopupMenuItem("Pause Queue", PauseQueue),
                    new PopupMenuItem("Shutdown Server", ShutdownServer),
                    new PopupMenuItem("Exit", ExitApp)
                }
            }
        };
        _trayIcon?.Create();
        _trayIcon?.Show();
#pragma warning restore CA1416
    }

    private static void PauseQueue(object? sender, EventArgs eventArgs)
    {
        // Implement the logic to pause the queue in NoMercy.Server
    }

    private static void ShutdownServer(object? sender, EventArgs eventArgs)
    {
        // Implement the logic to shutdown NoMercy.Server
    }

    private static void ExitApp(object? sender, EventArgs eventArgs)
    {
        // _trayIcon.Visible = false;
        Current?.Quit();
    }
}
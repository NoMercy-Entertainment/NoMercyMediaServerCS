// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Runtime.InteropServices;

namespace NoMercy.Server.StartupServices;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TrayIconFactory {
    public static Task MakeIcon()
    {
        // Todo add a system so we can tie the IconService into the rest of the server through DI 
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
            !OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18362)) return Task.CompletedTask;

        string iconPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/icon.ico");
        if (!File.Exists(iconPath)) {
            // Todo logging
            return Task.CompletedTask;
        }
            
        TrayIcon trayIcon = new(iconPath);
        trayIcon.Create();

        return Task.CompletedTask;
    }
}

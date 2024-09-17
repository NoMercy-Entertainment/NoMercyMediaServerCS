namespace NoMercy.Providers.File;
public class FileWatcherEventArgs(FileSystemWatcher? sender, FileSystemEventArgs fileSystemEventArgs) {
    // ReSharper disable once MemberCanBePrivate.Global
    public FileSystemEventArgs FileSystemEventArgs { get; private set; } = fileSystemEventArgs;
    public ErrorEventArgs? ErrorEventArgs { get; set; }
    public WatcherChangeTypes ChangeType => FileSystemEventArgs.ChangeType;

    public string Root { get; set; } = sender?.Path ?? string.Empty;
    public string Path { get; set; } = System.IO.Path.GetDirectoryName(fileSystemEventArgs.FullPath) ?? string.Empty;
    public string FullPath { get; set; } = fileSystemEventArgs.FullPath;
    public string? OldFullPath { get; set; } = (fileSystemEventArgs as RenamedEventArgs)?.OldFullPath;
    public FileSystemWatcher? Sender { get; set; } = sender;

}
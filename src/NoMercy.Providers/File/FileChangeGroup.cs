using NoMercy.Database.Models;

namespace NoMercy.Providers.File;
public class FileChangeGroup(WatcherChangeTypes type, Library library, string folderPath)
{
    public string FolderPath { get; set; } = folderPath;
    public Library Library { get; set; } = library;
    public WatcherChangeTypes ChangeType { get; set; } = type;
    public Timer? Timer { get; set; }
}
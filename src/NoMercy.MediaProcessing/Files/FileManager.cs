using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Jobs;
using NoMercy.NmSystem;

namespace NoMercy.MediaProcessing.Files;
public partial class FileManager(
    IFileRepository fileRepository,
    JobDispatcher jobDispatcher
) : IFileManager
{
    private int Id { get; set; }
    private Movie? Movie { get; set; }
    private Tv? Show { get; set; }

    private List<Folder> Folders { get; set; } = [];
    private List<MediaFolderExtend> Files { get; set; } = [];
    public string Type { get; set; } = "";

    public async Task FindFiles(int id, Library library)
    {
        Id = id;

        await MediaType(id, library);
        Folders = Paths(library, Movie, Show);

        foreach (Folder folder in Folders)
        {
            ConcurrentBag<MediaFolderExtend>? files = await GetFiles(library, folder.Path);

            if (!files.IsEmpty) Files.AddRange(files);
        }

        switch (library.Type)
        {
            case "movie":
                await StoreMovie();
                break;
            case "tv":
            case "anime":
                await StoreTvShow();
                break;
            case "music":
                await StoreMusic();
                break;
            default:
                Logger.App("Unknown library type");
                break;
        }
    }

    private async Task StoreMusic()
    {
        List<MediaFile> items = Files
            .SelectMany(file => file.Files ?? [])
            .Where(mediaFolder => mediaFolder.Parsed is not null)
            .ToList();

        if (items.Count == 0) return;

        foreach (MediaFile item in items) await StoreAudioItem(item);

        Logger.App($"Found {items.Count} music files");
    }

    private async Task StoreMovie()
    {
        MediaFile? item = Files
            .SelectMany(file => file.Files ?? [])
            .FirstOrDefault(file => file.Parsed is not null);

        if (item == null) return;

        await StoreVideoItem(item);

        Logger.App($"Found {item.Path} for {Movie?.Title}");
    }

    private async Task StoreTvShow()
    {
        List<MediaFile> items = Files
            .SelectMany(file => file.Files ?? [])
            .Where(mediaFolder => mediaFolder.Parsed is not null)
            .ToList();

        if (items.Count == 0) return;

        foreach (MediaFile item in items) await StoreVideoItem(item);

        Logger.App($"Found {items.Count} files for {Show?.Title}");
    }

    private async Task StoreAudioItem(MediaFile? item)
    {
        if (item?.Parsed is null) return;

        Folder? folder = Folders.FirstOrDefault(folder => item.Path.Contains(folder.Path));
        if (folder == null) return;

        await Task.CompletedTask;
    }

    private async Task StoreVideoItem(MediaFile item)
    {
        Folder? folder = Folders.FirstOrDefault(folder => item.Path.Contains(folder.Path));
        if (folder == null) return;

        string fileName = Path.DirectorySeparatorChar + Path.GetFileName(item.Path);
        string hostFolder = item.Path.Replace(fileName, "");
        string baseFolder = (Path.DirectorySeparatorChar + (Movie?.Folder ?? Show?.Folder ?? "").Replace("/", "")
                                                         + item.Path.Replace(folder.Path, ""))
                                                            .Replace(fileName, "");

        string subtitleFolder = Path.Combine(hostFolder, "subtitles");

        List<Subtitle> subtitles = [];

        if (Directory.Exists(subtitleFolder))
        {
            string[] subtitleFiles = Directory.GetFiles(subtitleFolder);
            foreach (string subtitleFile in subtitleFiles)
            {
                Regex regex = SubtitleFileRegex();
                Match match = regex.Match(subtitleFile);

                if (match.Groups["type"].Value != "sign" && match.Groups["type"].Value != "song" &&
                    match.Groups["type"].Value != "full") continue;

                subtitles.Add(new Subtitle
                {
                    Language = match.Groups["lang"].Value,
                    Type = match.Groups["type"].Value,
                    Ext = match.Groups["ext"].Value
                });
            }
        }

        Episode? episode = await fileRepository.GetEpisode(Show?.Id, item);

        List<IVideoTrack> tracks = [];
        
        string[] files = Directory.GetFiles(hostFolder);
        foreach (string file in files)
        {
            var name = Path.GetFileName(file);
            if (name.StartsWith("chapter"))
            {
                tracks.Add(new IVideoTrack
                {
                    File = "/" + name,
                    Kind = "chapters"
                });
            }
            else if (name.StartsWith("skipper"))
            {
                tracks.Add(new IVideoTrack
                {
                    File = "/" + name,
                    Kind = "skippers"
                });
            }
            else if ((name.StartsWith("sprite") || name.StartsWith("preview") || name.StartsWith("thumb")) && file.EndsWith("vtt"))
            {
                tracks.Add(new IVideoTrack
                {
                    File = "/" + name,
                    Kind = "thumbnails"
                });
            }
            else if ((name.StartsWith("sprite") || name.StartsWith("thumb")) && file.EndsWith("webp"))
            {
                tracks.Add(new IVideoTrack
                {
                    File = "/" + name,
                    Kind = "sprite"
                });
            }
            else if (name.StartsWith("fonts"))
            {
                tracks.Add(new IVideoTrack
                {
                    File = "/" + name,
                    Kind = "fonts"
                });
            }
        }
        
        try
        {
            Logger.App($"Storing video file: {episode?.Id}, {Movie?.Id}");
            VideoFile videoFile = new()
            {
                EpisodeId = episode?.Id,
                MovieId = Movie?.Id,
                Folder = baseFolder.Replace("\\", "/"),
                HostFolder = hostFolder.Replace("\\", "/"),
                Filename = fileName.Replace("\\", "/"),

                Share = folder.Id.ToString() ?? "",
                Duration = Regex.Replace(
                    Regex.Replace(item.FFprobe?.Duration.ToString() ?? ""
                        , "\\.\\d+", ""), "^00:", ""),
                // Chapters = JsonConvert.SerializeObject(item.FFprobe?.Chapters ?? []),
                Chapters = "",
                Languages = JsonConvert.SerializeObject(item.FFprobe?.AudioStreams.Select(stream => stream.Language)
                    .Where(stream => stream != null && stream != "und")),
                Quality = item.FFprobe?.VideoStreams.FirstOrDefault()?.Width.ToString() ?? "",
                Subtitles = JsonConvert.SerializeObject(subtitles),
                _tracks = JsonConvert.SerializeObject(tracks),
            };

            await fileRepository.StoreVideoFile(videoFile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    private async Task MediaType(int id, Library library)
    {
        (Movie, Show, Type) = await fileRepository.MediaType(id, library);
    }

    private async Task<ConcurrentBag<MediaFolderExtend>> GetFiles(Library library, string path)
    {
        MediaScan mediaScan = new();

        int depth = library.Type switch
        {
            "movie" => 1,
            "tv" or "anime" => 2,
            _ => 0
        };

        ConcurrentBag<MediaFolderExtend> folders = await mediaScan
            .EnableFileListing()
            .FilterByMediaType(library.Type)
            .Process(path, depth);

        await mediaScan.DisposeAsync();

        return folders;
    }

    private List<Folder> Paths(Library library, Movie? movie = null, Tv? show = null)
    {
        List<Folder> folders = new();
        string? folder = library.Type switch
        {
            "movie" => movie?.Folder?.Replace("/", ""),
            "tv" or "anime" => show?.Folder?.Replace("/", ""),
            _ => ""
        };

        if (folder == null) return folders;

        Folder[] rootFolders = library.FolderLibraries
            .Select(f => f.Folder)
            .ToArray();

        foreach (Folder rootFolder in rootFolders)
        {
            string path = Path.Combine(rootFolder.Path, folder);

            if (Directory.Exists(path))
                folders.Add(new Folder
                {
                    Path = path,
                    Id = rootFolder.Id
                });
        }

        return folders;
    }

    [GeneratedRegex(@"(?<lang>\w{3}).(?<type>\w{3,4}).(?<ext>\w{3})$")]
    private static partial Regex SubtitleFileRegex();
}
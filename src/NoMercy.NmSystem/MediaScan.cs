using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using FFMpegCore;
using MovieFileLibrary;
using Serilog.Events;
using DateTime = System.DateTime;

namespace NoMercy.NmSystem;

public class MovieFileExtend
{
    public string? Title { get; init; }
    public string? Year { get; init; }
    public bool IsSeries { get; set; }
    public int? Season { get; init; }
    public int? Episode { get; init; }
    public bool IsSuccess { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public int DiscNumber
    {
        get
        {
            if (Title is null) return 0;
            string pattern = @"^((?<discNumber>\d+)(-|\s))?(?<trackNumber>\d+)";
            Match match = Regex.Match(Title, pattern);
            return match.Groups["discNumber"].Success ? int.Parse(match.Groups["discNumber"].Value) : 0;
        }
    }
    
    public int TrackNumber
    {
        get
        {
            if (Title is null) return 0;
            string pattern = @"^((?<discNumber>\d+)-)?(?<trackNumber>\d+)";
            Match match = Regex.Match(Title, pattern);
            return match.Groups["trackNumber"].Success ? int.Parse(match.Groups["trackNumber"].Value) : 0;
        }
    }
}

public class MediaFolder
{
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; } = string.Empty;
    public MovieFileExtend? Parsed { get; init; }
}

public class MediaFolderExtend: MediaFolder
{
    public ConcurrentBag<MediaFile>? Files { get; init; } = [];
    public ConcurrentBag<MediaFolderExtend>? SubFolders { get; init; } = [];
}

public class FFprobeData
{
    public TimeSpan Duration { get; set; }
    public MediaFormat Format { get; set; }
    public AudioStream? PrimaryAudioStream { get; set; }
    public VideoStream? PrimaryVideoStream { get; set; }
    public SubtitleStream? PrimarySubtitleStream { get; set; }
    public List<VideoStream> VideoStreams { get; set; }
    public List<AudioStream> AudioStreams { get; set; }
    public List<SubtitleStream> SubtitleStreams { get; set; }
    public IReadOnlyList<string> ErrorData { get; set; }
}

public class MediaFile
{
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int Size { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; } = string.Empty;
    public MovieFileExtend? Parsed { get; init; }

    public FFprobeData? FFprobe { get; init; }
    // public Fingerprint? FingerPint { get; init; }
}

public class MediaScan : IDisposable, IAsyncDisposable
{
    private readonly MovieDetector _movieDetector = new();

    private bool _fileListingEnabled;
    private bool _regexFilterEnabled = true;

    private readonly Regex _folderNameRegex =
        new(
            @"video_.*|audio_.*|subtitles|scans|cds.*|ost|album|music|original|fonts|thumbs|metadata|NCED|NCOP|\s\(\d\)\.|~",
            RegexOptions.IgnoreCase);

    private string[] _extensionFilter = [];


    public MediaScan()
    {
        GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(AppFiles.BinariesPath, "ffmpeg"));
    }

    public MediaScan EnableFileListing()
    {
        _fileListingEnabled = true;

        return this;
    }

    public MediaScan DisableRegexFilter()
    {
        _regexFilterEnabled = false;

        return this;
    }

    public MediaScan FilterByMediaType(string mediaType)
    {
        _extensionFilter = mediaType switch
        {
            "anime" or "tv" or "movie" or "video" => [".mp4", ".avi", ".mkv", ".m3u8"],
            "music" => [".mp3", ".flac", ".wav", ".m4a"],
            "subtitle" => [".srt", ".vtt", ".ass"],
            _ => throw new ArgumentOutOfRangeException(nameof(mediaType), mediaType, null)
        };

        return this;
    }

    public Task<ConcurrentBag<MediaFolderExtend>> Process(string rootFolder, int depth = 0)
    {
        rootFolder = Path.GetFullPath(rootFolder.ToUtf8());
        return !_fileListingEnabled
            ? Task.Run(() => ScanFoldersOnly(rootFolder, depth))
            : Task.Run(() => ScanFolder(rootFolder, depth));
    }

    private ConcurrentBag<MediaFolderExtend> ScanFolder(string folderPath, int depth)
    {
        folderPath = Path.GetFullPath(folderPath.ToUtf8());

        ConcurrentBag<MediaFolderExtend>? folders = [];

        if (depth < 0) return folders;

        ConcurrentBag<MediaFile>? files = Files(folderPath);

        MovieFile movieFile1 = _movieDetector.GetInfo(folderPath);
        movieFile1.Year ??= Str.MatchYearRegex().Match(folderPath)
            .Value;

        folders.Add(new MediaFolderExtend
        {
            Name = Path.GetFileName(folderPath),
            Path = folderPath,
            Created = Directory.GetCreationTime(folderPath),
            Modified = Directory.GetLastWriteTime(folderPath),
            Accessed = Directory.GetLastAccessTime(folderPath),
            Type = "folder",
            Parsed = new MovieFileExtend
            {
                Title = movieFile1.Title,
                Year = movieFile1.Year,
                FilePath = movieFile1.Path,
            },

            Files = files
        });

        try
        {
            IOrderedEnumerable<string> directories = Directory.GetDirectories(folderPath).OrderBy(f => f);

            Parallel.ForEach(directories, (directory, _) =>
            {
                string folderName = Path.GetFileName(directory);

                if ((_regexFilterEnabled && _folderNameRegex.IsMatch(folderName)) || depth == 0)
                {
                    files.Add(new MediaFile
                    {
                        Name = folderName,
                        Path = directory,
                        Created = Directory.GetCreationTime(directory),
                        Modified = Directory.GetLastWriteTime(directory),
                        Accessed = Directory.GetLastAccessTime(directory),
                        Type = "folder"
                    });

                    return;
                }

                ConcurrentBag<MediaFile>? files2 = depth - 1 > 0 ? Files(directory) : [];

                MovieFile movieFile = _movieDetector.GetInfo(directory);
                movieFile.Year ??= Str.MatchYearRegex()
                    .Match(directory).Value;

                folders.Add(new MediaFolderExtend
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",
                    Parsed = new MovieFileExtend
                    {
                        Title = movieFile.Title,
                        Year = movieFile.Year,
                        FilePath = movieFile.Path
                    },

                    Files = files2.Count > 0
                        ? files2
                        : null,

                    SubFolders = depth - 1 > 0
                        ? ScanFolder(directory, depth - 1)
                        : []
                });
            });

            ConcurrentBag<MediaFolderExtend>? response = new(folders
                .Where(f => f.Name is not "")
                .OrderByDescending(f => f.Name));

            return response;
        }
        catch (Exception e)
        {
            Logger.App(e.Message, LogEventLevel.Fatal);
            throw;
        }
    }

    private ConcurrentBag<MediaFolderExtend> ScanFoldersOnly(string folderPath, int depth)
    {
        folderPath = Path.GetFullPath(folderPath.ToUtf8());

        if (depth < 0) return [];

        try
        {
            ConcurrentBag<MediaFolderExtend> folders = [];

            IOrderedEnumerable<string> directories = Directory.GetDirectories(folderPath).OrderBy(f => f);

            Parallel.ForEach(directories, (directory, _) =>
            {
                string dir = Path.GetFullPath(directory.ToUtf8());
                Logger.App($"Scanning {dir}");

                string folderName = Path.GetFileName(dir);

                if (_regexFilterEnabled && _folderNameRegex.IsMatch(folderName))
                {
                    folders.Add(new MediaFolderExtend
                    {
                        Name = folderName,
                        Path = dir,
                        Created = Directory.GetCreationTime(dir),
                        Modified = Directory.GetLastWriteTime(dir),
                        Accessed = Directory.GetLastAccessTime(dir),
                        Type = "folder"
                    });

                    return;
                }

                MovieFile movieFile = _movieDetector.GetInfo(directory);

                movieFile.Year ??= Str.MatchYearRegex()
                    .Match(directory).Value;

                folders.Add(new MediaFolderExtend
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",

                    Parsed = new MovieFileExtend
                    {
                        Title = movieFile.Title,
                        Year = movieFile.Year,
                        FilePath = movieFile.Path
                    },

                    SubFolders = depth - 1 > 0
                        ? ScanFoldersOnly(directory, depth - 1)
                        : []
                });
            });

            ConcurrentBag<MediaFolderExtend>? response = new(folders
                .Where(f => f.Name is not "")
                .OrderByDescending(f => f.Name));

            return response;
        }
        catch (Exception e)
        {
            Logger.App(e.Message, LogEventLevel.Fatal);
            throw;
        }
    }

    private ConcurrentBag<MediaFile>? Files(string folderPath)
    {
        try
        {
            ConcurrentBag<MediaFile> files = [];

            Parallel.ForEach(Directory.GetFiles(folderPath), (file, _) =>
            {
                file = Path.GetFullPath(file.ToUtf8());
                string extension = Path.GetExtension(file).ToLower();

                if (_extensionFilter.Length > 0 && !_extensionFilter.Contains(extension)) return;

                bool isVideoFile = extension is ".mp4" or ".avi" or ".mkv" or ".m3u8";
                bool isAudioFile = extension is ".mp3" or ".flac" or ".wav" or ".m4a";
                bool isSubtitleFile = extension is ".srt" or ".vtt" or ".ass" or ".sub";

                if (!isVideoFile && !isAudioFile && !isSubtitleFile) return;

                MovieFile? movieFile = isVideoFile || isAudioFile ? _movieDetector.GetInfo(file) : null;
                AnimeInfo animeInfo = AnimeParser.ParseAnimeFilename(file);

                if (movieFile is not null)
                {
                    movieFile.Year ??= Str.MatchYearRegex()
                        .Match(file).Value;
                    movieFile.Title ??= animeInfo.Name;
                    movieFile.Season ??= animeInfo.Season;
                    movieFile.Episode ??= animeInfo.Episode;
                }

                MovieFileExtend movieFileExtend = new()
                {
                    FilePath = movieFile?.Path ?? file,
                    Episode = movieFile?.Episode,
                    Year = movieFile?.Year,
                    Season = movieFile?.Season,
                    Title = movieFile?.Title,
                    IsSeries = movieFile?.IsSeries ?? false,
                    IsSuccess = movieFile?.IsSuccess ?? false
                };

                FFprobeData? ffprobe = null;
                try
                {
                    IMediaAnalysis analysis = FFProbe.Analyse(file);
                    if (isVideoFile || isAudioFile)
                    {
                        ffprobe = new FFprobeData
                        {
                            Duration = analysis.Duration,
                            Format = analysis.Format,
                            PrimaryAudioStream = analysis.PrimaryAudioStream,
                            PrimaryVideoStream = analysis.PrimaryVideoStream,
                            PrimarySubtitleStream = analysis.PrimarySubtitleStream,
                            VideoStreams = analysis.VideoStreams,
                            AudioStreams = analysis.AudioStreams,
                            SubtitleStreams = analysis.SubtitleStreams,
                            ErrorData = analysis.ErrorData
                        };
                    }
                }
                catch (Exception e)
                {
                    Logger.App(e.Message, LogEventLevel.Fatal);
                    return;
                }

                MediaFile res = new()
                {
                    Name = Path.GetFileName(file),
                    Path = file,
                    Extension = extension,
                    Size = (int)new FileInfo(file).Length,
                    Created = File.GetCreationTime(file),
                    Modified = File.GetLastWriteTime(file),
                    Accessed = File.GetLastAccessTime(file),
                    Type = "file",

                    Parsed = movieFileExtend,
                    FFprobe = ffprobe
                    // FingerPint = fingerPrint
                };

                files.Add(res);
            });

            ConcurrentBag<MediaFile>? response = new(files
                // .Where(f => f.Name is not "")
                .OrderBy(f => f.Name));

            return response;
        }
        catch (Exception e)
        {
            Logger.App(e.Message, LogEventLevel.Fatal);
            throw;
        }
    }

    public void Dispose()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();

        return ValueTask.CompletedTask;
    }
}
using MovieFileLibrary;
using Newtonsoft.Json;
using NoMercy.Helpers.Monitoring;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;

public class ServerInfoDto
{
    [JsonProperty("server")] public string Server { get; set; } = string.Empty;
    [JsonProperty("cpu")] public string Cpu { get; set; } = string.Empty;
    [JsonProperty("gpu")] public string[] Gpu { get; set; } = [];
    [JsonProperty("os")] public string Os { get; set; } = string.Empty;
    [JsonProperty("arch")] public string Arch { get; set; } = string.Empty;
    [JsonProperty("version")] public string? Version { get; set; }
    [JsonProperty("bootTime")] public DateTime BootTime { get; set; }
}

public class ServerPathsDto
{
    [JsonProperty("key")] public string Key { get; set; } = string.Empty;
    [JsonProperty("value")] public string Value { get; set; } = string.Empty;
}

public class DirectoryRequest
{
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
}

public class PathRequest
{
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
}

public class Video
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("width")] public int? Width { get; set; }
    [JsonProperty("height")] public int? Height { get; set; }
}

public class Audio
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("language")] public string? Language { get; set; } = string.Empty;
}

public class Subtitle
{
    [JsonProperty("index")] public int Index { get; set; }
    [JsonProperty("language")] public string? Language { get; set; }
}

public class FileListRequest
{
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}

public class DirectoryTreeDto
{
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("mode")] public int Mode { get; set; }
    [JsonProperty("size")] public int? Size { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("parent")] public string Parent { get; set; } = string.Empty;
    [JsonProperty("full_path")] public string FullPath { get; set; } = string.Empty;
}

public class SetupResponseDto
{
    [JsonProperty("setup_complete")] public bool SetupComplete { get; set; }
}

public class ServerUpdateRequest
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}

public class ResourceInfoDto
{
    [JsonProperty("cpu")] public Cpu Cpu { get; set; } = new();
    [JsonProperty("gpu")] public List<Gpu> Gpu { get; set; } = new();
    [JsonProperty("memory")] public Memory Memory { get; set; } = new();
    [JsonProperty("storage")] public List<ResourceMonitorDto> Storage { get; set; } = new();
}

public class FileListResponseDto
{
    [JsonProperty("status")] public string Status { get; set; } = string.Empty;
    [JsonProperty("files")] public List<FileItemDto> Files { get; set; } = new();
}

public class FileItemDto
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("mode")] public int Mode { get; set; }
    [JsonProperty("parent")] public string? Parent { get; set; }
    [JsonProperty("size")] public long Size { get; set; }
    [JsonProperty("parsed")] public MovieFile Parsed { get; set; } = new("");
    [JsonProperty("match")] public MovieOrEpisode Match { get; set; } = new();
    [JsonProperty("streams")] public Streams Streams { get; set; } = new();
    [JsonProperty("file")] public string File { get; set; } = string.Empty;
}

public class Streams
{
    [JsonProperty("video")] public IEnumerable<Video> Video { get; set; } = new List<Video>();
    [JsonProperty("audio")] public IEnumerable<Audio> Audio { get; set; } = new List<Audio>();
    [JsonProperty("subtitle")] public IEnumerable<Subtitle> Subtitle { get; set; } = new List<Subtitle>();
}

public class MovieOrEpisode
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
    [JsonProperty("duration")] public TimeSpan Duration { get; set; }
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("episode_number")] public int EpisodeNumber { get; set; }
    [JsonProperty("season_number")] public int SeasonNumber { get; set; }
    [JsonProperty("still")] public string? Still { get; set; }
}

public class AddFilesRequest
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("files")] public string[] Files { get; set; } = [];
}
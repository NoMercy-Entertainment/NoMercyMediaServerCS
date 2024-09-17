using Newtonsoft.Json;
using NoMercy.Database.Models;

namespace NoMercy.Data.Repositories;

public interface ILibraryRepository
{
    IQueryable<Library> GetLibrariesAsync(Guid userId);
    Task<Library> GetLibraryByIdAsync(Ulid libraryId, Guid userId, string language, int take, int page);
    IQueryable<Movie> GetLibraryMoviesAsync(Guid userId, Ulid libraryId, string language, int take, int page);
    IQueryable<Tv> GetLibraryShowsAsync(Guid userId, Ulid libraryId, string language, int take, int page);

    IOrderedQueryable<Library> GetDashboardLibrariesAsync(Guid userId);
    Task<Library?> GetLibraryByIdAsync(Ulid id);
    Task AddLibraryAsync(Library library, Guid userId);
    Task UpdateLibraryAsync(Library library);
    Task DeleteLibraryAsync(Library library);
    Task<List<Library>> GetAllLibrariesAsync();

    Task AddEncoderProfileFolderAsync(EncoderProfileFolder encoderProfileFolder);
    Task AddEncoderProfileFolderAsync(List<EncoderProfileFolder> encoderProfileFolders);
    Task AddEncoderProfileFolderAsync(EncoderProfileFolder[] encoderProfileFolders);
    Task AddLanguageLibraryAsync(LanguageLibrary[] languageLibraries);
    List<FolderDto> GetFoldersAsync();
}

public class LibraryUpdateRequest
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
    [JsonProperty("image")] public string? Image { get; set; }
    [JsonProperty("autoRefreshInterval")] public bool PerfectSubtitleMatch { get; set; } = true;
    [JsonProperty("realtime")] public bool Realtime { get; set; }
    [JsonProperty("specialSeasonName")] public string SpecialSeasonName { get; set; } = string.Empty;
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    [JsonProperty("folder_library")] public FolderLibraryDto[] FolderLibrary { get; set; } = [];
    [JsonProperty("subtitles")] public string[] Subtitles { get; set; } = [];
}

public class FolderLibraryDto
{
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder")] public FolderDto Folder { get; set; } = new();
}

public class FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("encoder_profiles")] public Ulid[] EncoderProfiles { get; set; } = [];
}

public class RescanLibraryRequest
{
    [JsonProperty("id")] public bool ForceUpdate { get; set; }
    [JsonProperty("synchronous")] public bool Synchronous { get; set; }
}

public class FolderRequest
{
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
}

public class LibrarySortRequest
{
    [JsonProperty("libraries")] public LibrarySortRequestItem[] Libraries { get; set; } = [];
}

public class LibrarySortRequestItem
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("order")] public int Order { get; set; }
}

public class ProfilesRequest
{
    [JsonProperty("profiles")] public string[] Profiles { get; set; } = [];
}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;

public record LibrariesDto
{
    [JsonProperty("data")] public IEnumerable<LibrariesResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, IAsyncEnumerable<Library?>> GetLibraries =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId) =>
            mediaContext.Libraries.AsNoTracking()
                .Where(library => library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null
                )
                .Include(library => library.FolderLibraries)
                .ThenInclude(folderLibrary => folderLibrary.Folder)
                .ThenInclude(folder => folder.EncoderProfileFolder)
                .ThenInclude(library => library.EncoderProfile)
                .Include(library => library.LanguageLibraries)
                .ThenInclude(languageLibrary => languageLibrary.Language));
}

public record LibrariesResponseItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("autoRefreshInterval")] public long AutoRefreshInterval { get; set; }
    [JsonProperty("chapterImages")] public long ChapterImages { get; set; }
    [JsonProperty("image")] public string? Image { get; set; }
    [JsonProperty("perfectSubtitleMatch")] public bool PerfectSubtitleMatch { get; set; }
    [JsonProperty("realtime")] public bool Realtime { get; set; }
    [JsonProperty("specialSeasonName")] public string? SpecialSeasonName { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }

    [JsonProperty("color_palette")] public object? ColorPalette { get; set; }

    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
    [JsonProperty("folder_library")] public FolderLibraryDto[] FolderLibrary { get; set; }
    [JsonProperty("subtitles")] public string[] Subtitles { get; set; }

    public LibrariesResponseItemDto(Library library)
    {
        string image = library.Type switch
        {
            "anime" => library.LibraryTvs?.Randomize()?.FirstOrDefault(lt => lt.Library.Type == "anime")?.Tv.Backdrop ??
                       library.Image ?? "",
            "movie" => library.LibraryMovies?.Randomize()?.FirstOrDefault(lm => lm.Library.Type == "movie")?.Movie
                .Backdrop ?? library.Image ?? "",
            "tv" => library.LibraryTvs?.Randomize()?.FirstOrDefault(lt => lt.Library.Type == "tv")?.Tv.Backdrop ??
                    library.Image ?? "",
            _ => library.Image ?? ""
        };

        Id = library.Id;
        AutoRefreshInterval = library.AutoRefreshInterval;
        Image = image;
        PerfectSubtitleMatch = library.PerfectSubtitleMatch;
        Realtime = library.Realtime;
        SpecialSeasonName = library.SpecialSeasonName;
        Title = library.Title;
        Type = library.Type;
        Order = library.Order;
        CreatedAt = library.CreatedAt;
        UpdatedAt = library.UpdatedAt;

        Subtitles = library.LanguageLibraries
            .Select(languageLibrary => languageLibrary.Language.Iso6391)
            .ToArray();

        FolderLibrary = library.FolderLibraries
            .Select(folderLibrary => new FolderLibraryDto
            {
                FolderId = folderLibrary.FolderId,
                LibraryId = folderLibrary.LibraryId,
                Folder = new FolderDto
                {
                    Id = folderLibrary.Folder.Id,
                    Path = folderLibrary.Folder.Path,
                    EncoderProfiles = folderLibrary.Folder.EncoderProfileFolder
                        .Select(encoderProfileFolder => encoderProfileFolder.EncoderProfile.Id)
                        .ToArray()
                }
            })
            .ToArray();
    }
}

public record FolderLibraryDto
{
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder")] public FolderDto Folder { get; set; }
}

public record FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("encoder_profiles")] public Ulid[] EncoderProfiles { get; set; }
}
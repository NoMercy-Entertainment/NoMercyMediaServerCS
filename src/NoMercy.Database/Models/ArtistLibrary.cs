#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(ArtistId), nameof(LibraryId))]
[Index(nameof(ArtistId))]
[Index(nameof(LibraryId))]
public class ArtistLibrary
{
    [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    public Library Library { get; set; }

    public ArtistLibrary()
    {
    }

    public ArtistLibrary(Guid artistId, Ulid libraryId)
    {
        ArtistId = artistId;
        LibraryId = libraryId;
    }
}
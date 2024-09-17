#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(LibraryId), nameof(TrackId))]
[Index(nameof(LibraryId))]
[Index(nameof(TrackId))]
public class LibraryTrack
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    public Library Library { get; set; }

    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public LibraryTrack()
    {
    }

    public LibraryTrack(Ulid libraryId, Guid trackId)
    {
        LibraryId = libraryId;
        TrackId = trackId;
    }
}
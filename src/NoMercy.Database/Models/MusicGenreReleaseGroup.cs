#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(GenreId), nameof(ReleaseGroupId))]
[Index(nameof(GenreId))]
[Index(nameof(ReleaseGroupId))]
public class MusicGenreReleaseGroup
{
    [JsonProperty("genre_id")] public Guid GenreId { get; set; }
    public MusicGenre Genre { get; set; }

    [JsonProperty("track_id")] public Guid ReleaseGroupId { get; set; }
    public ReleaseGroup ReleaseGroup { get; set; }

    public MusicGenreReleaseGroup()
    {
    }

    public MusicGenreReleaseGroup(Guid genreId, Guid trackId)
    {
        GenreId = genreId;
        ReleaseGroupId = trackId;
    }
}
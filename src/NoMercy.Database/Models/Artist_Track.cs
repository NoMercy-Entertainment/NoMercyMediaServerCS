#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(ArtistId), nameof(TrackId))]
[Index(nameof(ArtistId))]
[Index(nameof(TrackId))]
public class ArtistTrack
{
    [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public ArtistTrack()
    {
    }

    public ArtistTrack(Guid artistId, Guid trackId)
    {
        ArtistId = artistId;
        TrackId = trackId;
    }
}
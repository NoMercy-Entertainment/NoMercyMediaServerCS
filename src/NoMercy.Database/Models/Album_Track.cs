#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(AlbumId), nameof(TrackId))]
[Index(nameof(AlbumId))]
[Index(nameof(TrackId))]
public class AlbumTrack
{
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    public Album Album { get; set; }

    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public AlbumTrack()
    {
    }

    public AlbumTrack(Guid albumId, Guid trackId)
    {
        AlbumId = albumId;
        TrackId = trackId;
    }
}
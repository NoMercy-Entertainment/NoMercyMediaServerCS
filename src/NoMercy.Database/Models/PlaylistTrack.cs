#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(PlaylistId), nameof(TrackId))]
[Index(nameof(PlaylistId))]
[Index(nameof(TrackId))]
public class PlaylistTrack
{
    [JsonProperty("playlist_id")] public Ulid PlaylistId { get; set; }
    public Playlist Playlist { get; set; }

    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public PlaylistTrack()
    {
    }

    public PlaylistTrack(Ulid playlistId, Guid trackId)
    {
        PlaylistId = playlistId;
        TrackId = trackId;
    }
}
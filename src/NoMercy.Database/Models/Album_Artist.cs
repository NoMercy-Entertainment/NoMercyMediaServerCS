#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(AlbumId), nameof(ArtistId))]
[Index(nameof(AlbumId))]
[Index(nameof(ArtistId))]
public class AlbumArtist
{
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    public Album Album { get; set; }

    [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    public AlbumArtist()
    {
    }

    public AlbumArtist(Guid albumId, Guid artistId)
    {
        AlbumId = albumId;
        ArtistId = artistId;
    }
}
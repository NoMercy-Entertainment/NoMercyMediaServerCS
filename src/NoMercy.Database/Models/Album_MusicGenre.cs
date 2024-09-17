#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(AlbumId), nameof(MusicGenreId))]
[Index(nameof(AlbumId))]
[Index(nameof(MusicGenreId))]
public class AlbumMusicGenre
{
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    public Album Album { get; set; }

    [JsonProperty("music_genre_id")] public Guid MusicGenreId { get; set; }
    public MusicGenre MusicGenre { get; set; }

    public AlbumMusicGenre()
    {
    }

    public AlbumMusicGenre(Guid albumId, Guid musicGenreId)
    {
        AlbumId = albumId;
        MusicGenreId = musicGenreId;
    }
}
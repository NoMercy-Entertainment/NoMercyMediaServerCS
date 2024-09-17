#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(ArtistId), nameof(MusicGenreId))]
[Index(nameof(ArtistId))]
[Index(nameof(MusicGenreId))]
public class ArtistMusicGenre
{
    [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    [JsonProperty("music_genre_id")] public Guid MusicGenreId { get; set; }
    public MusicGenre MusicGenre { get; set; }

    public ArtistMusicGenre()
    {
    }

    public ArtistMusicGenre(Guid artistId, Guid musicGenreId)
    {
        ArtistId = artistId;
        MusicGenreId = musicGenreId;
    }
}
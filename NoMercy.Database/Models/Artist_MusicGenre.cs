﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(MusicGenreId))]
    public class ArtistMusicGenre
    {
        [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [JsonProperty("music_genre_id")] public Guid MusicGenreId { get; set; }
        public virtual MusicGenre MusicGenre { get; set; }

        public ArtistMusicGenre()
        {
        }

        public ArtistMusicGenre(Guid artistId, Guid musicGenreId)
        {
            ArtistId = artistId;
            MusicGenreId = musicGenreId;
        }
    }
}
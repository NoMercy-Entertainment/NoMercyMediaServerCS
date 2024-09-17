#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchGenres
{
    [JsonProperty("music_genre_list")] public MusixMatchMusicGenreList[] MusicGenreList { get; set; }
}

public class MusixMatchMusicGenreList
{
    [JsonProperty("music_genre")] public MusixMatchMusicGenre MusixMatchMusicGenre { get; set; }
}
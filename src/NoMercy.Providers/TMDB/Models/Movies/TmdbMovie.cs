using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovie : TmdbBase
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("genres")] public int[]? GenresIds { get; set; } = [];
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }
    [JsonProperty("tagline")] public string? Tagline { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
    [JsonProperty("video")] public string? Video { get; set; }
}
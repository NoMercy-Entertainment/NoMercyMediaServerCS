using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieAlternativeTitles
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("titles")] public TmdbMovieAlternativeTitle[] Results { get; set; } = [];
}

public class TmdbMovieAlternativeTitle
{
    [JsonProperty("iso_3166_1")] public string? Iso31661 { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
}
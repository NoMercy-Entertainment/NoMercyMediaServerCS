using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieReleaseDates
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbReleaseDatesResult[] Results { get; set; } = [];
}

public class TmdbReleaseDatesResult
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;
    [JsonProperty("release_dates")] public TmdbReleaseDate[] ReleaseDates { get; set; } = [];
}

public class TmdbReleaseDate
{
    [JsonProperty("certification")] public string Certification { get; set; } = string.Empty;
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; } = string.Empty;
    [JsonProperty("release_date")] public DateTime ReleaseDateReleaseDate { get; set; } = DateTime.MinValue;

    [JsonProperty("type")] public int Type { get; set; }
    [JsonProperty("note")] public string? Note { get; set; }
}
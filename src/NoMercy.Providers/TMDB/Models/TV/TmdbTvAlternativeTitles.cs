#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvAlternativeTitles
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbTvAlternativeTitle[] Results { get; set; } = [];
}

public class TmdbTvAlternativeTitle
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
}
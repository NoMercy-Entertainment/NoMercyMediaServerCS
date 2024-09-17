#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvContentRatings
{
    [JsonProperty("results")] public TmdbTvContentRating[] Results { get; set; }
}

public class TmdbTvContentRating
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("rating")] public string Rating { get; set; }
    [JsonProperty("descriptors")] public string[] Descriptors { get; set; }
}
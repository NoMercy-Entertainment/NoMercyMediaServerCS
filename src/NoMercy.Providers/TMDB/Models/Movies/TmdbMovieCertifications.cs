using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Movies;

public class TmdbMovieCertifications
{
    [JsonProperty("results")] public MovieCertification[] Results { get; set; }
}

public class MovieCertification
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("rating")] public string Rating { get; set; }
    [JsonProperty("descriptors")] public string[] Descriptors { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;
public class TmdbMovieCertification
{
    [JsonProperty("certification")] public string? Rating { get; set; }
    [JsonProperty("meaning")] public string Meaning { get; set; }
    [JsonProperty("order")] public int Order { get; set; }
}
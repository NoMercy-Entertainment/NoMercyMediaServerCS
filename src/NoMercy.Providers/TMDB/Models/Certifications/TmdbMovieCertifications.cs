using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class TmdbMovieCertifications
{
    [JsonProperty("certifications")]
    public Dictionary<string, TmdbMovieCertification[]> Certifications { get; set; } = new();
}
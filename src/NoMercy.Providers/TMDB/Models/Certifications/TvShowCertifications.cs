using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Certifications;

public class TvShowCertifications
{
    [JsonProperty("certifications")]
    public Dictionary<string, TmdbTvShowCertification[]> Certifications { get; set; } = new();
}
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Shared;
public class TmdbSharedTranslationData
{
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("homepage")] public Uri Homepage { get; set; }
    [JsonProperty("tagline")] public string Tagline { get; set; }
}
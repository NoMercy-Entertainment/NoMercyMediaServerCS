using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Combined;
public class TmdbCombinedTranslation
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("english_name")] public string EnglishName { get; set; }
    [JsonProperty("data")] public TmdbCombinedTranslationData Data { get; set; }
}
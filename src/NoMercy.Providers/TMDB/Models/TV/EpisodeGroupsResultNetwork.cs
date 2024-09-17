using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;
public class EpisodeGroupsResultNetwork
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("logo_path")] public string LogoPath { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin_country")] public string OriginCountry { get; set; }
}
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Networks;

namespace NoMercy.Providers.TMDB.Models.TV;
public class TmdbEpisodeGroupsResult
{
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
    [JsonProperty("group_count")] public int GroupCount { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("network")] public TmdbNetwork TmdbNetwork { get; set; }
    [JsonProperty("type")] public int Type { get; set; }
}
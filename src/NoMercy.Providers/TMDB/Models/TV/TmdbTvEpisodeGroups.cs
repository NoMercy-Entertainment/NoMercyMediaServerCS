#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Networks;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvEpisodeGroups
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbEpisodeGroupsResult[] Results { get; set; } = [];
}

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

public class EpisodeGroupsResultNetwork
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("logo_path")] public string LogoPath { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin_country")] public string OriginCountry { get; set; }
}
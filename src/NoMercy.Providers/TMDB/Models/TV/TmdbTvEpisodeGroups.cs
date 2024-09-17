#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TmdbTvEpisodeGroups
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbEpisodeGroupsResult[] Results { get; set; } = [];
}
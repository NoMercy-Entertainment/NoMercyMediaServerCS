#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbRole
{
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("character")] public string Character { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
}
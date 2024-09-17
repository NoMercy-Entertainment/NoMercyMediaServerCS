using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbJob
{
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("job")] public string JobJob { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
}
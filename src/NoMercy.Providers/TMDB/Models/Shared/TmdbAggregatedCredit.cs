#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbAggregatedCredit
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("gender")] public int Gender { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }
    [JsonProperty("popularity")] public float Popularity { get; set; }
    [JsonProperty("profile_path")] public string? ProfilePath { get; set; }
    [JsonProperty("order")] public int Order { get; set; }
}

public class TmdbTmdbAggregatedCrew : TmdbAggregatedCredit
{
    [JsonProperty("jobs")] public TmdbAggregatedCrewJob[] Jobs { get; set; } = [];
}

public class TmdbAggregatedCrewJob
{
    [JsonProperty("credit_id")] public string? CreditId { get; set; }
    [JsonProperty("job")] public string? Job { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }
}

public class TmdbTmdbAggregatedCast : TmdbAggregatedCredit
{
    [JsonProperty("roles")] public TmdbAggregatedCreditRole[] Roles { get; set; } = [];
}

public class TmdbAggregatedCreditRole
{
    [JsonProperty("credit_id")] public string? CreditId { get; set; }
    [JsonProperty("character")] public string? Character { get; set; }
    [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }
}
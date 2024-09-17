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
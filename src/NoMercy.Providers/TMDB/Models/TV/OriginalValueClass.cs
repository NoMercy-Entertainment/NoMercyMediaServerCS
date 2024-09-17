using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;
public class OriginalValueClass
{
    [JsonProperty("id")] public int? Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("person_id")] public int? PersonId { get; set; }
    [JsonProperty("season_id")] public int? SeasonId { get; set; }
    [JsonProperty("poster")] public TmdbPoster TmdbPoster { get; set; }
    [JsonProperty("department")] public string Department { get; set; }
    [JsonProperty("job")] public string Job { get; set; }
}
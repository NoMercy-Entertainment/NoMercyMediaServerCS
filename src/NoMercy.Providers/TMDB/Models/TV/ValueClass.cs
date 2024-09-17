using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;
public class ValueClass
{
    [JsonProperty("season_id")] public int? SeasonId { get; set; }
    [JsonProperty("season_number")] public int? SeasonNumber { get; set; }
    [JsonProperty("id")] public int? Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("add_to_every_season")] public bool? AddToEverySeason { get; set; }
    [JsonProperty("character")] public string Character { get; set; }
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }
    [JsonProperty("person_id")] public int? PersonId { get; set; }
    [JsonProperty("poster")] public TmdbPoster TmdbPoster { get; set; }
    [JsonProperty("department")] public string Department { get; set; }
    [JsonProperty("job")] public string Job { get; set; }
}
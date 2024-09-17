using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;
public class TmdbTvChangeItem
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("action")] public Action Action { get; set; }
    [JsonProperty("time")] public string Time { get; set; }
    [JsonProperty("value")] public string? Value { get; set; }
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
    [JsonProperty("original_value")] public string? OriginalValue { get; set; }
}
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Networks;

public class TmdbNetworkAlternativeNames
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbNetworkAlternativeNamesResult[] Results { get; set; } = [];
}

public class TmdbNetworkAlternativeNamesResult
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}
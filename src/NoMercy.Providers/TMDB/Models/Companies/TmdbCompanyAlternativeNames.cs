using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Companies;

public class AlternativeNames
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TmdbAlternativeNameTmdbResult[] Results { get; set; } = [];
}

public class TmdbAlternativeNameTmdbResult
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}
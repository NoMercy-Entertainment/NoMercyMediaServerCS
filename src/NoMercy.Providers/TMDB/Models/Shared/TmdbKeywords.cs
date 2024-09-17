using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class TmdbSharedKeywords
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public virtual TmdbKeyword[] Results { get; set; } = [];
}

public class TmdbKeyword
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}
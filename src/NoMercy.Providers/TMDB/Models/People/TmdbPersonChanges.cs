using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class TmdbPersonChanges
{
    [JsonProperty("changes")] public TmdbPersonChange[] Changes { get; set; } = [];
}

public class TmdbPersonChange
{
    [JsonProperty("key")] public string Key { get; set; } = string.Empty;
    [JsonProperty("items")] public TmdbPersonChangeItem[] Items { get; set; } = [];
}

public class TmdbPersonChangeItem
{
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;
    [JsonProperty("action")] public string Action { get; set; } = string.Empty;
    [JsonProperty("time")] public string Time { get; set; } = string.Empty;

    [JsonProperty("original_value")]
    public TmdbPersonChangeOriginalValue TmdbPersonChangeOriginalValue { get; set; } = new();
}

public class TmdbPersonChangeOriginalValue
{
    [JsonProperty("profile")] public string Profile { get; set; } = string.Empty;
}
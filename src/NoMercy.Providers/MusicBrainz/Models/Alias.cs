using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class Alias : MusicBrainzLifeSpan
{
    [JsonProperty("locale")] public string? Locale { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("primary")] public bool? Primary { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}
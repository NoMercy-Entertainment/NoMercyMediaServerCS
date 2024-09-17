using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class Collection
{
    [JsonProperty("editor")] public string Editor { get; set; }
    [JsonProperty("entity-type")] public string EntityType { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("release-count")] public int ReleaseCount { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}
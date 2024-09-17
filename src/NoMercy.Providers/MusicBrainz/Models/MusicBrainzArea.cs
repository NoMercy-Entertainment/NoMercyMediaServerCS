using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzArea
{
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("type")] public object Type { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("iso-3166-1-codes")] public string[] Iso31661Codes { get; set; }
    [JsonProperty("iso-3166-2-codes")] public string[] Iso31662Codes { get; set; }
}
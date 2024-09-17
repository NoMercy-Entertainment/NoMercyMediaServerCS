using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzLabel
{
    [JsonProperty("aliases")] public Alias[] Aliases { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("genres")] public MusicBrainzGenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("label-code")] public string? LabelCode { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("tags")] public MusicBrainzTag[] Tags { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}
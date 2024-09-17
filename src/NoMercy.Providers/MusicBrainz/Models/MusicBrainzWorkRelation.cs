using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzWorkRelation : MusicBrainzLifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public MusicBrainzAttributeValues MusicBrainzAttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("artist")] public MusicBrainzArtist MusicBrainzArtist { get; set; }
    [JsonProperty("label")] public MusicBrainzLabel MusicBrainzLabel { get; set; }
    [JsonProperty("url")] public MusicBrainzUrl MusicBrainzUrl { get; set; }
}
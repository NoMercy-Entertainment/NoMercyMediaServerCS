using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class RecordingRelation : MusicBrainzLifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public MusicBrainzAttributeValues MusicBrainzAttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("artist")] public PurpleArtist Artist { get; set; }
    [JsonProperty("attribute-credits")] public AttributeCredits AttributeCredits { get; set; }
    [JsonProperty("label")] public PurpleArtist Label { get; set; }
    [JsonProperty("work")] public MusicBrainzWork MusicBrainzWork { get; set; }
    [JsonProperty("recording")] public RelationRecording Recording { get; set; }
}
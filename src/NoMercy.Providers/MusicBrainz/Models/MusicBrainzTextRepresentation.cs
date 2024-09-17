using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzTextRepresentation
{
    [JsonProperty("script")] public string Script { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
}
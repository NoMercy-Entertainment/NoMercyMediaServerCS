using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;
public class AttributeCredits
{
    [JsonProperty("Rhodes piano")] public string RhodesPiano { get; set; }
    [JsonProperty("synthesizer")] public string Synthesizer { get; set; }
    [JsonProperty("drums (drum set)")] public string? DrumsDrumSet { get; set; }
    [JsonProperty("handclaps")] public string Handclaps { get; set; }
    [JsonProperty("Hammond organ")] public string HammondOrgan { get; set; }
    [JsonProperty("keyboard")] public string Keyboard { get; set; }
    [JsonProperty("drum machine")] public string DrumMachine { get; set; }
    [JsonProperty("foot stomps")] public string FootStomps { get; set; }

    [JsonProperty("Wurlitzer electric piano")]
    public string WurlitzerElectricPiano { get; set; }
}
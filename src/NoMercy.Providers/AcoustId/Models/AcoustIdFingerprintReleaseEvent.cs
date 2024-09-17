using Newtonsoft.Json;

namespace NoMercy.Providers.AcoustId.Models;
public class AcoustIdFingerprintReleaseEvent
{
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("date")] public AcoustIdFingerprintDate Date { get; set; }
}
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.AcoustId.Models;

public class AcoustIdFingerprint
{
    [JsonProperty("results")] public AcoustIdFingerprintResult[] Results { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
}
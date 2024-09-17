using Newtonsoft.Json;

namespace NoMercy.Providers.AcoustId.Client;
public class FingerPrintData
{
    [JsonProperty("duration")] public double _duration { get; set; }
    [JsonProperty("fingerprint")] public string Fingerprint { get; set; }

    public int Duration
    {
        get => (int)Math.Floor(_duration);
        set => _duration = value;
    }
}
using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusicBrainz.Models;
public class MusicBrainzLifeSpan
{
    [JsonProperty("begin")] private string? _beginSpan { get; set; }

    public DateTime? BeginDate {
        get => !string.IsNullOrWhiteSpace(_beginSpan) && !string.IsNullOrEmpty(_beginSpan) && _beginSpan.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _beginSpan = value.ToString();
    }

    [JsonProperty("end")] private string? _endSpan { get; set; }

    public DateTime? EndDate
    {
        get => !string.IsNullOrWhiteSpace(_endSpan) && !string.IsNullOrEmpty(_endSpan) && _endSpan.TryParseToDateTime(out DateTime dt) ? dt : null;
        set => _endSpan = value.ToString();
    }

    [JsonProperty("ended")] public bool Ended { get; set; }
}